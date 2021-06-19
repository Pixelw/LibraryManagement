using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Chapter12_winform.utils {
    public class PrintHelper {
        #region Members //成员

        public String printName = String.Empty;
        public Font prtDataFont = new Font("Arial", 10);
        public Font prtTitleFont = new Font("微软雅黑", 23, FontStyle.Bold);
        public Font prtColumnHeaderFont = new Font("Arial", 10, FontStyle.Bold);
        private String[] titles = new String[0];

        public String[] Titles {
            set {
                titles = value as String[];
                if (null == titles) {
                    titles = new String[0];
                }
            }
            get { return titles; }
        }

        private Int32 leftMargin = 0;
        private Int32 topMargin = 0;

        public Int32 TopMargin {
            set { topMargin = value; }
            get { return topMargin; }
        }

        public Int32 LeftMargin {
            set { leftMargin = value; }
            get { return leftMargin; }
        }

        private DataTable printedTable;
        private Int32 pheight;
        private Int32 pWidth;
        private Int32 pindex;
        private Int32 curdgi;
        private Int32[] width;
        private Int32 rowOfDownDistance = 3;
        private Int32 rowOfUpDistance = 3;
        private Int32 gapBetweenContentAndTitle = 50;

        Int32 startColumnControls = 0;
        Int32 endColumnControls = 0;

        #endregion

        #region Method for PrintDataTable //打印数据集

        /// <summary>
        /// 打印数据集
        /// </summary>
        /// <param name="table">数据集</param>
        /// <returns></returns>
        public Boolean PrintDataTable(DataTable table) {
            PrintDocument prtDocument = new PrintDocument();
            try {
                if (printName != String.Empty) {
                    prtDocument.PrinterSettings.PrinterName = printName;
                }

                prtDocument.DefaultPageSettings.Landscape = false;
                prtDocument.OriginAtMargins = true;
                PrintDialog prtDialog = new PrintDialog();
                prtDialog.AllowSomePages = true;
                prtDialog.ShowHelp = false;
                prtDialog.Document = prtDocument;
                if (DialogResult.OK != prtDialog.ShowDialog()) {
                    return false;
                }

                printedTable = table;
                pindex = 0;
                curdgi = 0;
                width = new Int32[printedTable.Columns.Count];
                pheight = prtDocument.PrinterSettings.DefaultPageSettings.Bounds.Bottom + 400;
                //pheight = prtDocument.PrinterSettings.DefaultPageSettings.Bounds.Bottom;
                pWidth = prtDocument.PrinterSettings.DefaultPageSettings.Bounds.Right;
                prtDocument.PrintPage += docToPrint_PrintPage;
                prtDocument.Print();
            }
            catch (InvalidPrinterException ex) {
                MessageBox.Show("没有安装打印机\n" + ex.Message);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

            prtDocument.Dispose();
            return true;
        }

        #endregion

        #region Handler for docToPrint_PrintPage //设置打印机开始打印的事件处理函数

        /// <summary>
        /// 设置打印机开始打印的事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void docToPrint_PrintPage(object sender, PrintPageEventArgs ev) //设置打印机开始打印的事件处理函数
        {
            Int32 x = leftMargin;
            Int32 y = topMargin;
            Int32 i;
            Pen pen = new Pen(Brushes.Black, 1);
            if (0 == pindex) {
                // 画标题
                for (i = 0; i < titles.Length; i++) {
                    y += rowOfUpDistance;
                    ev.Graphics.DrawString(titles[i], prtTitleFont, Brushes.Black, leftMargin, y);
                    y += prtTitleFont.Height + rowOfDownDistance;
                }

                //margin
                y += gapBetweenContentAndTitle;
                foreach (DataRow dr in printedTable.Rows) {
                    for (i = 0; i < printedTable.Columns.Count; i++) {
                        Int32 colwidth =
                            Convert.ToInt16(ev.Graphics.MeasureString(dr[i].ToString().Trim(), prtColumnHeaderFont).Width);
                        if (colwidth > width[i]) {
                            width[i] = colwidth;
                        }
                    }
                }
            }
            
            y += rowOfUpDistance;
            // 画列头
            for (i = endColumnControls; i < printedTable.Columns.Count; i++) {
                String headtext = printedTable.Columns[i].ColumnName.Trim();
                if (pindex == 0) {
                    int colwidth = Convert.ToInt16(ev.Graphics.MeasureString(headtext, prtColumnHeaderFont).Width);
                    if (colwidth > width[i]) {
                        width[i] = colwidth;
                    }
                }

                //判断宽是否越界
                if (x + width[i] > pWidth) {
                    break;
                }


                ev.Graphics.DrawString(headtext, prtColumnHeaderFont, Brushes.Black, x, y);
                x += width[i];
                
            }
            y += rowOfDownDistance + prtColumnHeaderFont.Height;
            
            ev.Graphics.DrawLine(pen, leftMargin, y, x, y);
            int rowOfTop = y;

            startColumnControls = endColumnControls;
            if (i < printedTable.Columns.Count) {
                endColumnControls = i;
                ev.HasMorePages = true;
            }
            else {
                endColumnControls = printedTable.Columns.Count;
            }
            
            //打印数据
            for (i = curdgi; i < printedTable.Rows.Count; i++) {
                x = leftMargin;
                y += rowOfUpDistance;
                for (Int32 j = startColumnControls; j < endColumnControls; j++) {
                    ev.Graphics.DrawString(printedTable.Rows[i][j].ToString().Trim(), prtDataFont, Brushes.Black, x,
                        y);
                    x += width[j];
                }

                y += prtDataFont.Height + rowOfDownDistance;
                ev.Graphics.DrawLine(pen, leftMargin, y, x, y);
                //判断高是否越界
                if (y > pheight - prtDataFont.Height - 400) //if (y > pWidth - prtTextFont.Height - 30)
                {
                    break;
                }
            }

            ev.Graphics.DrawLine(pen, leftMargin, rowOfTop, leftMargin, y);
            x = leftMargin;
            for (Int32 k = startColumnControls; k < endColumnControls; k++) {
                x += width[k];
                ev.Graphics.DrawLine(pen, x, rowOfTop, x, y);
            }

            //判断高是否越界
            if (y > pheight - prtDataFont.Height - 400) //if (y > pWidth - prtTextFont.Height - 30) 
            {
                pindex++;
                if (0 == startColumnControls) {
                    curdgi = i + 1;
                }

                if (!ev.HasMorePages) {
                    endColumnControls = 0;
                }

                ev.HasMorePages = true;
            }
        }

        #endregion
    }
}