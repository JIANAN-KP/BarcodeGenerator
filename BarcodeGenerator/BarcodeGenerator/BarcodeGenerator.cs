using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Office;
using Microsoft.Office.Interop;


namespace BarcodeGenerator
{
    public partial class BarcodeGenerator : Form
    {
        public const string rootDir = "C:\\temp\\BarcodeGenerator\\BCGen\\";
        public const string logDir = "C:\\temp\\BarcodeGenerator\\BCLog.txt";
        public const string debugDir = "C:\\temp\\BarcodeGenerator\\BCDebug.txt";
        public const string igFolder = "C:\\VWorks Workspace\\Barcode Input Files";
        public const string tAssayFile = "AssayNexar.csv";
        public const string tDaughtFile = "DaugNexar.csv";
        public const string tExtFile = "Ext.csv";
        public const string starStr = "****************************";
        public const string appName = "Barcode Generator";
        public const string bigLabelPrinter = "172.20.194.31";
        public const string smallLabelPrinter = "172.20.194.30";
        public const int MPLXONEMAXASSAY = 22;
        public const int MPLXTWOMAXASSAY = 44;
        public const int MPLXFOURMAXASSAY = 88;

        public string vMplxNo = string.Empty;
        public string strSpace = " ";
        public string strDot = ".";
        public List<PrjItem> prjList;
        public bool isDebug = true;
        public string folderName = string.Empty;

        public BarcodeGenerator()
        {
            InitializeComponent();
            prjList = new List<PrjItem>();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTaskFile_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            string selectFileName = string.Empty;
            string expCode = string.Empty;
            string techName = string.Empty;
            List<ExpObj> expList = new List<ExpObj>();
            string extractType = string.Empty;
            set_folder_name();
            if((cbTest.Enabled==true)&&(cbTest.Checked == true))
            {
                isDebug = true;
            }else
            {
                isDebug = false;
                this.Cursor = Cursors.WaitCursor;
                CheckPrinterConnection(IPAddress.Parse(bigLabelPrinter));
                CheckPrinterConnection(IPAddress.Parse(smallLabelPrinter));
                this.Cursor = Cursors.Default;
            }

            DialogResult fileResult = FolderBrowserDialog.ShowDialog();
            if(fileResult.Equals(DialogResult.OK))
            {
                path = FolderBrowserDialog.SelectedPath;
            }
            else
            {
                return;
            }

            tbSystemMsg.Text = tbSystemMsg.Text + "*** BARCODE GENERATOR STARTED! *** " + '\n';
            if (fileResult.Equals(DialogResult.OK))
            {
                DirectoryInfo dInfo = new DirectoryInfo(path);
                FileInfo fileInfo = dInfo.GetFiles("*.txt").First();
                selectFileName = fileInfo.Name;
                string logger = " [ " + System.DateTime.Today + " ] >>> " + "select file: " + selectFileName;
                expCode = selectFileName.Substring(0, 9);
                string[] lines = File.ReadAllLines(fileInfo.FullName);
                foreach (string line in lines)
                {
                    string[] strs = line.Split('\t');
                    ExpObj expObj = new ExpObj();
                    expObj.srcTaskId = strs[0].ToString();
                    expObj.dstTaskId = strs[1].ToString();
                    expObj.pDef = strs[2].ToString();
                    expObj.pName = strs[3].ToString();
                    expObj.assayName = strs[5].ToString();
                    if (strs[6] != null && strs[6].Length > 0)
                    {
                        expObj.mplxNo = strs[6].ToString();
                    }
                    else
                    {
                        expObj.mplxNo = string.Empty;
                    }
                    expObj.dstPlateNo = strs[7].ToString();
                    expObj.srcPlateNo = strs[8].ToString();

                    /* get technology name (do it only once) */
                    if (techName.Length.Equals(0))
                    {
                        if (expObj.pName.ToUpper().Contains("7900"))
                        {
                            techName = "7900";
                        }
                        else if (expObj.pName.ToUpper().Contains("SNP GA"))
                        {
                            techName = "NEXAR";
                        }
                        else
                        {
                            techName = string.Empty;
                        }
                    }

                    /* get extraction type (do it only once) */
                    if (extractType.Length.Equals(0))
                    {
                        if (expObj.pName.ToUpper().Contains("EXTRACT N"))
                        {
                            extractType = "NORMAL96";
                        }
                        else if (expObj.pName.ToUpper().Contains("48 TO 96 WELL"))
                        {
                            extractType = "NEW48TO96";
                        }
                        else
                        {
                            extractType = string.Empty;
                        }
                    }
                    expList.Add(expObj);
                }
                printLog(starStr);
                printLog(" [ " + System.DateTime.Today + " ] >>> " + "parsed technology:" + techName);
                printLog(" [ " + System.DateTime.Today + " ] >>> " + "parsed extraction type:" + extractType);

                /*
                    '*** get mplx no ***
                    'ver 1.6    - The logic to determine the mplx no should be determined by the number of daughter plates in column I of the task list file (eg. 1,2,4,5 = mplx 1; 1,2 = mplx 2 and 1 = mplx 4
                    */

                int blksPerDaught = (from r in expList where r.pName.Contains("DAUGH") select r.srcPlateNo).First().Split().Count();
                /* determine the mplx no for the experiment */
                if (blksPerDaught.Equals(1))
                {
                    vMplxNo = "4";
                }
                else if (blksPerDaught.Equals(2))
                {
                    vMplxNo = "2";
                }
                else
                {
                    vMplxNo = "1";
                }

                /*
                    'log = "Mplx No : " + vMplxNo
                    'logger.writeline("[" & System.DateTime.Today & "] >>> " & log)
                    'only proceed when there are records to begin printing
                    */
                if (expList.Count > 0)
                {
                    tbSystemMsg.Text = tbSystemMsg.Text + "Experiment Code: " + expCode + '\n';
                    tbSystemMsg.Text = tbSystemMsg.Text + "Technology: " + techName + '\n';
                    if (cbExtraction.Checked == true)
                    {
                        print_Extraction(expList, expCode, extractType);
                    }
                    if (techName.ToUpper().Equals("7900"))
                    {
                        if (cbDaughter.Checked == true)
                        {
                            //print_7900_Daughter(expList, expCode, techName);
                        }
                        if (cbAssay.Checked == true)
                        {
                            // temporary not needed;
                            // print_7900_Assay(expList, expCode, techName);
                        }
                    }
                    else if (techName.ToUpper().Equals("NEXAR"))
                    {
                        if (cbDaughter.Checked == true)
                        {
                            print_Nexar_Daught(expList, expCode, techName);
                        }
                        if (cbAssay.Checked == true)
                        {
                            print_Nexar_Assay(expList, expCode, techName, path);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No records found in the task file!Please verify and try again later.");
                }
            }
            else
            {
                MessageBox.Show("No file has been selected! Please try again.");
                printLog(" [ " + System.DateTime.Today + " ] >>> " + "WARN: No file has been selected!");
            }
            tbSystemMsg.Text = tbSystemMsg.Text + "*** BARCODE GENERATOR ENDED! ***" + '\n';
            clear_folder_name();
        }

        private void clear_folder_name()
        {
            this.folderName = string.Empty;
        }

        private void set_folder_name()
        {
            folderName = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
        }

        private void print_Extraction(List<ExpObj> explist,string expCode,string extractType)
        {
            /*
                ' Barcode Format:
                ' ---------------
                ' <Barcode>
                ' <Magenta Exp Code>-<Plate Num>
                ' <Sampling Plate Id>/<Daughter Plate Id>  <Month> <Country>
                '
                ' ExtractType : NORMAL96
                ' Eg.  | |||| ||| || | ||| || |
                '      NXT001C01-11
                '      1/(A)21/(N)45  APR/09  SG
                ' where 'A' and 'N' denotes Assay and Nexar plate no
                '
                ' ExtractType : NEW48TO96
                ' Eg.  | |||| ||| || | ||| || |
                '      NXT001C01-11
                '      1,2/(N)45      APR/09  SG
                ' where 'N' denotes Nexar plate no

                'declaration
             */
            IEnumerable<ExpObj> extractRes = Enumerable.Empty<ExpObj>();
            string printText = string.Empty;
            int vDistance = 40;
            string strLine1 = string.Empty;
            string strLine2 = string.Empty;
            string strLine3 = string.Empty;

            if (extractType.ToUpper().Equals("NORMAL96"))
            {
                extractRes = from rec in explist
                             where rec.pName.ToUpper().Contains("EXTRACT")
                             orderby Convert.ToInt32(rec.dstPlateNo) ascending
                             select rec;
            }
            else if(extractType.ToUpper().Equals("NEW48TO96"))
            {
                extractRes = from rec in explist
                             where rec.pName.ToUpper().Contains("48 TO 96 WELL")
                             orderby Convert.ToInt32(rec.dstPlateNo) ascending
                             select rec;
            }
  
            printLog(" [ " + System.DateTime.Today + " ] >>> " + "No of extraction records: " + extractRes.Count());
            tbSystemMsg.Text = tbSystemMsg.Text + "Parsing extraction labels..." + '\n';

            progressBar.Maximum = extractRes.Count();
            progressBar.Value = 0;

            List<ExtractionItem> listExt = new List<ExtractionItem>();
            if(extractRes.Count()>0)
            {
                if(isDebug)
                {
                    printDebugMsg("^XA");
                    printDebugMsg("^SZ2^JMA");
                    printDebugMsg("^MCY^PMN");
                    printDebugMsg("^PW500");
                    printDebugMsg("^MD29");
                    printDebugMsg("^JZY");
                    printDebugMsg("^XZ");
                }
                printText = "^XA^SZ2^JMA^MCY^PMN^PW1000^MD29^JZY^XZ";
                foreach (ExpObj expObj in extractRes)
                {
                    string code = expCode.ToUpper().ToString() + "-" + expObj.dstPlateNo.ToString();
                    //string srcPlatNo = "(" + expObj.srcPlateNo + ")";
                    string srcPlatNo = expObj.srcPlateNo;
                    string dateStr = dtPicker.Value.Date.ToString("dd/MM/yy");
                    // string dateStr = DateTime.Now.ToString("dd/MM/yy");

                    ExtractionItem extItem = new ExtractionItem();
                    extItem.prjName = code;
                    extItem.date = dateStr;
                    extItem.extNo = "="+"\""+"("+reforExtStr(srcPlatNo)+")"+"\"";
                    listExt.Add(extItem);
                    if (isDebug)
                    {
                        printDebugMsg("Extraction Debug Msg");
                        printDebugMsg("^XA");
                        printDebugMsg("^LH0,0^LRN");
                        printDebugMsg("^XZ");
                        printDebugMsg("^XA");
                        printDebugMsg("^FO" + (30 + vDistance) + ",55");
                    }
                    printText = printText + "^XA^PQ1,0,1,Y^XZ";
                    //line 1 -- Write barcode
                    printText = printText + "^XA^LH0,0^LRN^XZ^XA^FO" + (15 + vDistance) + ",30";
                    strLine1 = "^FT" + (30 + vDistance) + ",30^A0,N,18,30^FD" + extItem.prjName + "^FS";
                    strLine1 = strLine1 + "^FT" + (250 + vDistance) + ",30^A0,N,18,30^FD" + extItem.date + "^FS";
                    strLine1 = strLine1 + "^FT" + (400 + vDistance) + ",30^A0,N,18,30^FD" + "(" + reforExtStr(srcPlatNo) + ")" + "^FS";
                    if (isDebug)
                    {
                        printDebugMsg(strLine1);
                    }
                    printText = printText + strLine1;
                    strLine2 = "^FT170,60^BY1^BCN,30,N,N^FD" + extItem.prjName;
                    if(isDebug)
                    {
                        printDebugMsg(strLine2);
                    }
                    printText = printText + strLine2+ "^XZ";

                    if (isDebug)
                    {
                        printDebugMsg("^XA");
                        printDebugMsg("^PQ1,0,1,Y");
                        printDebugMsg("^XZ");
                    }
                    printLog("["+System.DateTime.Today+" ]>>> "+ "extraction barcode to be printed: "+expCode.ToUpper().ToString()+"-"+expObj.dstPlateNo.ToString());
                    progressBar.Value = progressBar.Value + 1;                   
                }
                if ((!isDebug)&&(rbPrinter.Checked==true))
                {
                    sendToPrint(printText.ToString(), IPAddress.Parse(smallLabelPrinter));
                    printLog("[" + System.DateTime.Today + "] >>> " + "extraction printing starts");
                }
                else
                {
                    printLog("[" + System.DateTime.Today + "] >>> " + "extraction printing not started");
                }
            }
            else
            {
                if(!isDebug)
                {
                    MessageBox.Show("No extraction labels found for printing!");
                }
                else
                {
                    printDebugMsg("No extraction labels found for printing!");
                }
            }

            /* generate csv file */
            if ((listExt.Count() > 0) && (rbLabeler.Checked == true))
            {
                /* check folder exist, if not create folder first */

                string folderPath = rootDir + folderName;
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileStr = "\\Ext-" + expCode + ".csv";
                string fileDir = folderPath + fileStr;
                var csvText = new StringBuilder();
                var newLine = string.Format("{0},{1},{2}", "prjName", "date", "extNo");
                csvText.AppendLine(newLine);
                foreach (ExtractionItem extItem in listExt)
                {
                    newLine = string.Format("{0},{1},{2}", extItem.prjName, extItem.date, extItem.extNo);
                    csvText.AppendLine(newLine);
                }
                File.WriteAllText(fileDir, csvText.ToString());
                attachExtItemsToCSV(listExt);
            }


        }

        private void print_Nexar_Assay(List<ExpObj> sampleList, string expCode, string techName,string path)
        {
            string printText = string.Empty;
            int vDistance = 15;
            string strLine1 = string.Empty;
            string strLine2 = string.Empty;
            List<FileInfo> listInfos = new List<FileInfo>();
            DirectoryInfo dInfo = new DirectoryInfo(path);
            List<FileInfo> assayfiles = dInfo.GetFiles("*-*PV*.xml").ToList();
            IEnumerable<ExpObj> recList = Enumerable.Empty<ExpObj>();
            recList = from s in sampleList.ToArray()
                      select s;
            tbSystemMsg.Text = tbSystemMsg.Text + "...parsed multiplex no: " + vMplxNo + '\n';
            listInfos.Clear();
            foreach(FileInfo fInfo in assayfiles)
            {
                if(fInfo!=null)
                {
                    listInfos.Add(fInfo);
                }
            }
            listInfos = listInfos.SortFiles();
            if (isDebug)
            {
                printDebugMsg("^XA");
                printDebugMsg("^SZ2^JMA");
                printDebugMsg("^MCY^PMN");
                printDebugMsg("^PW1000");
                printDebugMsg("^MD29");
                printDebugMsg("^JZY");
                printDebugMsg("^XZ");
            }
            printText = "^XA^SZ2^JMA^MCY^PMN^PW1000^MD29^JZY^XZ";
            List<AssayItem> listAssays = new List<AssayItem>();
            IEnumerable<FileObj> assayList = Enumerable.Empty<FileObj>();
            string mplxNo = string.Empty;
            /*
            foreach(ExpObj expObj in sampleList)
            {
                if(!expObj.mplxNo.Equals(string.Empty))
                {
                    mplxNo = expObj.mplxNo;
                }
            }
            */
            if(listInfos.Count()>0)
            {
                foreach (FileInfo fileInfo in listInfos)
                {
                    XDocument doc = XDocument.Load(fileInfo.FullName);
                    printLog("[" + System.DateTime.Today + "] >>> " + "selected file's fullname: " + fileInfo.FullName + " vs short name: " + fileInfo.Name);

                    int assayCount = (from well in doc.Descendants("well")
                                      where well.Element("assay").Value != "BLANK"
                                      select well).Count();
                    int noOfDistinctAssay = (from well in doc.Descendants("well")
                                             where well.Element("assay").Value != "BLANK"
                                             select well.Element("assay").Value).Distinct().Count();
                    double noOfRepeats = 0.0;
                    if ((assayCount / noOfDistinctAssay) >= 4)
                    {
                        noOfRepeats = Math.Round((assayCount / noOfDistinctAssay / 4) + 0.25);
                        mplxNo = "1";
                    }
                    else if ((assayCount / noOfDistinctAssay) == 2)
                    {
                        noOfRepeats = Math.Round((assayCount / noOfDistinctAssay / 2) + 0.25);
                        mplxNo = "2";
                    }
                    else
                    {
                        noOfRepeats = Math.Round((assayCount / noOfDistinctAssay) + 0.25);
                        mplxNo = "4";
                    }
                    if (isDebug)
                    {
                        printDebugMsg("^XA");
                        printDebugMsg("^LH0,0^LRN");
                        printDebugMsg("^XZ");
                        printDebugMsg("^XA");
                        printDebugMsg("^FO" + (15 + vDistance) + ",30");
                    }

                    printText = printText + "^XA^PQ1,0,1,Y^XZ";
                    AssayItem assayItem = new AssayItem();
                    assayItem.prjName = fileInfo.Name.Substring(0, fileInfo.Name.IndexOf(".xml"));
                    assayItem.assayNo = noOfDistinctAssay.ToString() + "Assaysx" + Convert.ToInt32(noOfRepeats).ToString();
                    assayItem.mplxNo = "M" + mplxNo;
                    listAssays.Add(assayItem);

                    printText = printText + "^XA^LH0,0^LRN^XZ^XA^FO" + (15 + vDistance) + ",30";
                    strLine1 = "^FT" + (30 + vDistance) + ",30^A0,N,18,30^FD" + fileInfo.Name.Substring(0, fileInfo.Name.IndexOf(".xml")) + "^FS";
                    strLine1 = strLine1 + "^FT" + (250 + vDistance) + ",30^A0,N,18,30^FD" + assayItem.assayNo + "^FS";
                    strLine1 = strLine1 + "^FT" + (400 + vDistance) + ",30^A0,N,18,30^FD" + assayItem.mplxNo + "^FS";
                    tbSystemMsg.Text = tbSystemMsg.Text + "...assay label: " + fileInfo.Name.Substring(0, fileInfo.Name.IndexOf(".xml")) + '\n';
                    if (isDebug)
                    {
                        printDebugMsg(strLine1);
                    }
                    printText = printText + strLine1;
                    strLine2 = "^FT170,60^BY1^BCN,30,N,N^FD" + fileInfo.Name.Substring(0, fileInfo.Name.IndexOf(".xml"));
                    if (isDebug)
                    {
                        printDebugMsg(strLine2);
                        printDebugMsg("^XZ");
                    }
                    printText = printText + strLine2 + "^XZ";
                    if (isDebug)
                    {
                        printDebugMsg("^XA");
                        printDebugMsg("^PQ1,0,1,Y");
                        printDebugMsg("^XZ");
                    }
                   
                }
                if ((!isDebug)&&(rbPrinter.Checked==true))
                {
                    sendToPrint(printText.ToString(), IPAddress.Parse(smallLabelPrinter));
                    printLog("[" + System.DateTime.Today + "] >>> " + "nexar assay printing starts");
                    tbSystemMsg.Text = tbSystemMsg.Text + "nexar assay printing starts..." + '\n';
                }
                else
                {
                    printLog("[" + System.DateTime.Today + "] >>> " + "nexar assay printing not started");
                    tbSystemMsg.Text = tbSystemMsg.Text + "nexar assay printing not started!" + '\n';
                }
            }
            else
            {
                if (!isDebug)
                {
                    MessageBox.Show("No assay files found for printing!");
                }
                else
                {
                    printDebugMsg("No assay files found for printing!");
                }
            }

         
            if ((listAssays.Count() > 0) && (rbLabeler.Checked == true))
            {
                string folderPath = rootDir + folderName;
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileStr = "\\AssayNexar-" + expCode + ".csv";
                string fileDir = folderPath + fileStr;
                var csvText = new StringBuilder();
                var newLine = string.Format("{0},{1},{2}", "prjName", "assayNo", "mplxNo");
                csvText.AppendLine(newLine);
                foreach (AssayItem assayItem in listAssays)
                {
                    newLine = string.Format("{0},{1},{2}", assayItem.prjName, assayItem.assayNo, assayItem.mplxNo);
                    csvText.AppendLine(newLine);
                }
                File.WriteAllText(fileDir, csvText.ToString());
                attachAssayItemsToCSV(listAssays);
            }
            
        }

        private void print_7900_Daughter(List<ExpObj> expList,string expCode,string techName)
        {
            string printText = string.Empty;
            string strLine1 = string.Empty;
            string strLine2 = string.Empty;
            int vDistance = 15;
            string firstDaught = string.Empty;
            IEnumerable<ExpObj> daughtRes = Enumerable.Empty<ExpObj>();
            daughtRes = from rec in expList
                        where rec.pName.ToUpper().ToString().Contains("DAUGH")
                        orderby Convert.ToInt32(rec.dstPlateNo) ascending
                        select rec;
            if(isDebug)
            {
                printDebugMsg("No of daughter records = " + daughtRes.Count());
            }
            printLog(" [ " + System.DateTime.Today + " ] >>> " + "No of daughter records: "+daughtRes.Count());
            tbSystemMsg.Text = tbSystemMsg.Text + "Parsing 7900 daughter labels..." + '\n';
            progressBar.Maximum = daughtRes.Count();
            progressBar.Value = 0;
            List<DaughterItem> listDaug = new List<DaughterItem>();
            if(daughtRes.Count()>0)
            {
                if(isDebug)
                {
                    printDebugMsg("^XA");
                    printDebugMsg("^SZ2^JMA");
                    printDebugMsg("^MCY^PMN");
                    printDebugMsg("^PW1000");
                    printDebugMsg("^MD29");
                    printDebugMsg("^JZY");
                    printDebugMsg("^XZ");
                }
                printText = "^XA^SZ2^JMA^MCY^PMN^PW1000^MD29^JZY^XZ";
                foreach(ExpObj expObj in daughtRes)
                {
                    int blksPerDaught = (from r in daughtRes
                                         where (r.pName.Contains("DAUGH") && (r.dstPlateNo.ToString().Equals(expObj.dstPlateNo.ToString()))
                                         || (r.srcTaskId.ToString().Equals(expObj.srcTaskId.ToString())))
                                         select r.srcPlateNo).First().Split().Count();
                    if(blksPerDaught.Equals(1))
                    {
                        vMplxNo = "4";
                    }else if(blksPerDaught.Equals(2))
                    {
                        vMplxNo = "2";
                    }
                    else
                    {
                        vMplxNo = "1";
                    }
                    //'generate printer specified commands and files
                    if(isDebug)
                    {
                        printDebugMsg("^XA");
                        printDebugMsg("^LH0,0^LRN");
                        printDebugMsg("^XZ");
                        printDebugMsg("^XA");
                        printDebugMsg("^FO" + (15 + vDistance) + ",30");
                    }
                    printText = printText + "^XA^LH0,0^LRN^XZ^XA^FO" + (15 + vDistance) + ",30";
                    if(firstDaught.Length.Equals(0))
                    {
                        firstDaught = expObj.dstPlateNo;
                    }
                    strLine1 = "^FT" + (30 + vDistance) + ",30^A0,N,18,30^FD" + expCode.ToUpper().ToString() + "-" + expObj.dstPlateNo.ToString() + "^FS";
                    if(isDebug==true)
                    {
                        printDebugMsg(strLine1);
                    }
                    printText = printText + strLine1;
                    // Generate barcode
                    strLine2 = "^^FT170,60^BCN,30,N,N^FD" + expCode.ToUpper().ToString() + "-" + expObj.dstPlateNo.ToString();
                    if(isDebug)
                    {
                        printDebugMsg(strLine2);
                        printDebugMsg("^XZ");
                    }
                    printText = printText + strLine2 + "^XZ";
                    printLog("[" + System.DateTime.Today + "] >>> "+ "daughter barcode to be printed: "+expCode.ToUpper().ToString()+"-"+expObj.dstPlateNo.ToString());

                    DaughterItem daItem = new DaughterItem();
                    daItem.prjName = expCode.ToUpper().ToString() + "-" + expObj.dstPlateNo.ToString();
                    daItem.multiplex = "M" + vMplxNo;
                    daItem.extNo = (Convert.ToInt32(expObj.dstPlateNo) - (Convert.ToInt32(firstDaught) - 1)) / Convert.ToInt32(expObj.srcPlateNo)+"";
                    daItem.arrNo = "Arr:";
                    listDaug.Add(daItem);

                    if(isDebug)
                    {
                        printDebugMsg("^XA");
                        printDebugMsg("^PQ1,0,1,Y");
                        printDebugMsg("^XZ");
                    }
                    printText = printText + "^XA^PQ1,0,1,Y^XZ";
                    progressBar.Value = progressBar.Value + 1;
                }
                
                if(!isDebug)
                {
                    sendToPrint(printText.ToString(), IPAddress.Parse(smallLabelPrinter));
                    printLog("[" + System.DateTime.Today + "] >>> "+ "daughter printing starts");
                }
                else
                {
                    printLog("[" + System.DateTime.Today + "] >>> " + "daughter printing not started");
                }
            }
            else
            {
                if(!isDebug)
                {
                    MessageBox.Show("No 7900 daughter labels found for printing!");
                }
                else
                {
                    printDebugMsg("No 7900 daughter labels found for printing!");
                }
            }
            /* generate csv file */
            if ((listDaug.Count() > 0) && (rbLabeler.Checked == true))
            {
                string delimiter = "\t";
                string fileDir = "Daug7900-" + rootDir + DateTime.Now.ToString("yyyy - MM - dd_hhmmss") + ".txt";
                using (FileStream fs = new FileStream(fileDir, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter sr = new StreamWriter(fs))
                    {
                        string title = "prjName" + delimiter + "multiplex" + delimiter + "extNo" + delimiter + "arrNo";
                        sr.WriteLine(title);
                        foreach (DaughterItem itemDaug in listDaug)
                        {
                            string strLine = itemDaug.prjName + delimiter + itemDaug.multiplex + delimiter + itemDaug.extNo + delimiter + itemDaug.arrNo;
                            sr.WriteLine(strLine);
                        }
                    }
                }
            }
        }

        private void print_7900_Assay(List<ExpObj> sampleList, string expCode, string techName)
        {
            string printText = string.Empty;
            string strLine1 = string.Empty;
            string strLine2 = string.Empty;
            int vDistance = 15;

            IEnumerable<ExpObj> assayRes = from rec in sampleList
                                           where rec.pName.ToUpper().ToString().Contains("SNP")
                                           orderby Convert.ToInt32(rec.dstPlateNo) ascending
                                           select rec;
            if(isDebug)
            {
                printDebugMsg("No of assay records = " + assayRes.Count());
            }
            tbSystemMsg.Text = tbSystemMsg.Text + "Parsing 7900 assay labels..." + '\n';
            progressBar.Maximum = assayRes.Count();
            progressBar.Value = 0;
            List<AssayItem> listAssays = new List<AssayItem>();
            if(assayRes.Count()>0)
            {
                if(isDebug)
                {
                    printDebugMsg("^XA");
                    printDebugMsg("^SZ2^JMA");
                    printDebugMsg("^MCY^PMN");
                    printDebugMsg("^PW1000");
                    printDebugMsg("^MD29");
                    printDebugMsg("^JZY");
                    printDebugMsg("^XZ");
                }
                printText = "^XA^SZ2^JMA^MCY^PMN^PW1000^MD29^JZY^XZ";
                foreach(ExpObj expObj in assayRes)
                {
                    if(isDebug)
                    {
                        printDebugMsg("^XA");
                        printDebugMsg("^LH0,0^LRN");
                        printDebugMsg("^XZ");
                        printDebugMsg("^XA");
                        printDebugMsg("^FO" + (15 + vDistance) + ",30");
                    }
                    printText = printText + "^XA^LH0,0^LRN^XZ^XA^FO" + (15 + vDistance) + ",30";
                    strLine1 = "^FT" + (30 + vDistance) + ",30^A0,N,18,30^FD" + expCode.ToUpper().ToString() + "-" + expObj.dstPlateNo.ToString();
                    if(isDebug)
                    {
                        printDebugMsg(strLine1);
                    }
                    printText = printText + strLine1;
                    strLine2 = "^FT170,60^BCN,30,N,N^FD" + expCode.ToUpper().ToString() + "-" + expObj.dstPlateNo.ToString();
                    if(isDebug)
                    {
                        printDebugMsg(strLine2);
                        printDebugMsg("^XZ");
                    }
                    printText = printText + strLine2 + "^XZ";

                    AssayItem assayItem = new AssayItem();
                    assayItem.prjName = expCode.ToUpper().ToString() + "-" + expObj.dstPlateNo.ToString();
                    listAssays.Add(assayItem);

                    if (isDebug)
                    {
                        printDebugMsg("^XA");
                        printDebugMsg("^PQ1,0,1,Y");
                        printDebugMsg("^XZ");
                    }
                    printText = printText + "^XA^PQ1,0,1,Y^XZ";
                    progressBar.Value = progressBar.Value + 1;
                }
                if(isDebug)
                {
                    printLog("[" + System.DateTime.Today + "] >>> " + "assay printing not started");
                }
                else
                {
                    sendToPrint(printText.ToString(), IPAddress.Parse(smallLabelPrinter));
                    printLog("[" + System.DateTime.Today + "] >>> " + "assay printing starts");
                }
            }else
            {
                if(isDebug)
                {
                    printDebugMsg("No 7900 assay labels found for printing!");
                }
                else
                {
                    MessageBox.Show("No 7900 assay labels found for printing!");
                }
            }

            /* generate csv file */
            if ((listAssays.Count() > 0) && (rbLabeler.Checked == true))
            {
                string delimiter = "\t";
                string fileDir = "Daug7900-" + rootDir + DateTime.Now.ToString("yyyy - MM - dd_hhmmss") + ".txt";
                using (FileStream fs = new FileStream(fileDir, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter sr = new StreamWriter(fs))
                    {
                        string title = "prjName" + delimiter + "multiplex" + delimiter + "extNo" + delimiter + "arrNo";
                        sr.WriteLine(title);
                       // foreach (DaughterItem itemDaug in listAssays)
                       // {
                       //     string strLine = itemDaug.prjName + delimiter + itemDaug.multiplex + delimiter + itemDaug.extNo + delimiter + itemDaug.arrNo;
                      //      sr.WriteLine(strLine);
                       // }
                    }
                }
            }
        }

        private void print_Nexar_Daught(List<ExpObj> expList,string expCode,string techName)
        {
            string printText = string.Empty;
            string strLine1 = string.Empty;
            string strLine2 = string.Empty;
            int vDistance = 15;
            int noOfAssayByDaught = 0;
            int noOfRepeatedArrayByDaught = 0;
            IEnumerable<ExpObj> daughtAndAssayRes = Enumerable.Empty<ExpObj>();
            daughtAndAssayRes = from rec in expList
                                where rec.pName.ToUpper().ToString().Contains("DAUGH") ||
                                rec.pName.ToUpper().ToString().Contains("SNP")
                                select rec;
            if(isDebug)
            {
                printDebugMsg("No of daught and snp records = " + daughtAndAssayRes.Count());
            }
            tbSystemMsg.Text = tbSystemMsg.Text + "...parsed multiplex no: " + vMplxNo + '\n';
            if(isDebug)
            {
                printDebugMsg("^XA");
                printDebugMsg("^SZ2^JMA");
                printDebugMsg("^MCY^PMN");
                printDebugMsg("^PW1000");
                printDebugMsg("^MD29");
                printDebugMsg("^JZY");
                printDebugMsg("^XZ");
            }
            printText = "^XA^SZ2^JMA^MCY^PMN^PW1000^MD29^JZY^XZ";

            string textLine = string.Empty;
            List<DaughterItem> listDaugs = new List<DaughterItem>();
            if(daughtAndAssayRes.Count()>0)
            {
                foreach (ExpObj expObj in daughtAndAssayRes)
                {
                    if (expObj.pName.ToString().ToUpper().Contains("DAUGH"))
                    {
                        if (cbDaughter.Checked == true)
                        {
                            IEnumerable<ExpObj> assayRowByTaskRes = Enumerable.Empty<ExpObj>();
                            assayRowByTaskRes = from assayRec in daughtAndAssayRes
                                                where assayRec.pName.ToUpper().ToString().Contains("SNP GA") && assayRec.srcPlateNo.ToString().Equals(expObj.dstPlateNo.ToString())
                                                select assayRec;
                            noOfAssayByDaught = assayRowByTaskRes.Count();

                            int blksPerDaught = (from r in daughtAndAssayRes
                                                 where r.pName.Contains("DAUGH") && r.dstPlateNo.ToString().Equals(expObj.dstPlateNo.ToString()) || r.srcTaskId.ToString().Equals(expObj.srcTaskId.ToString())
                                                 select r.srcPlateNo).First().Split(',').Count();
                            //determine the mplx no for the experiment  
                            if (blksPerDaught.Equals(1))
                            {
                                vMplxNo = "4";
                            }
                            else if (blksPerDaught.Equals(2))
                            {
                                vMplxNo = "2";
                            }
                            else
                            {
                                vMplxNo = "1";
                            }
                            if (vMplxNo.Equals("1"))
                            {
                                //get repeat no (no of repeated array within a task regardless the number of pv)
                                noOfRepeatedArrayByDaught = (int)Math.Ceiling(noOfAssayByDaught / 1.0);
                            }
                            else if (vMplxNo.Equals("2"))
                            {
                                //get repeat no (no of repeated array within a task regardless the number of pv)
                                noOfRepeatedArrayByDaught = (int)Math.Ceiling(noOfAssayByDaught / 2.0);
                            }
                            else if (vMplxNo.Equals("4"))
                            {
                                //get repeat no (no of repeated array within a task regardless the number of pv)
                                noOfRepeatedArrayByDaught = (int)Math.Ceiling(noOfAssayByDaught / 4.0);
                            }
                            /*
                                Ver 1.5    - Incorporating logic to generate more than 1 daughter labels if the number of assays is less
                                           than the available daughter plates. Eg. for mplx 1 of 768 samples (8 blocks) and 314 assays, 
                                           no of daughter plates will be 2 in the task list file, but there will be 6 in the recipes. 
                                           This is because (768 samples / 4) = 2 x 384 daughter plates; 314 assays of mplx 1 split into 
                                           2 daughter plates (157 each) will not be enough because the number of arrays that can be ran
                                           through the pipe is 66. Therefore, the number of daughter plates will be 157 / 66 = 3 (rounded).
                             */
                            int maxArrayPerNexarRun = 66;
                            double temD = (double)noOfRepeatedArrayByDaught / maxArrayPerNexarRun;
                            int noOfAddDaughterPlates = (int)Math.Ceiling(temD);
                            int j = 0;

                            while (j < noOfAddDaughterPlates)
                            {
                                j = j + 1;
                                if (isDebug)
                                {
                                    printDebugMsg("^XA");
                                    printDebugMsg("^LH0,0^LRN");
                                    printDebugMsg("^XZ");
                                    printDebugMsg("^XA");
                                    printDebugMsg("^FO" + (15 + vDistance) + ",30");
                                }
                                DaughterItem item = new DaughterItem();
                                item.prjName = expCode.ToUpper().ToString() + "-" + expObj.dstPlateNo + "V" + j.ToString();
                                item.multiplex = "M" + vMplxNo.ToString();
                                item.extNo = reforExtStr(expObj.srcPlateNo.ToString());
                                item.arrNo = "Arr:" + noOfRepeatedArrayByDaught.ToString();
                                listDaugs.Add(item);

                                textLine = "<" + expCode.ToUpper().ToString() + "-" + expObj.dstPlateNo + "V" + j.ToString()
                                    + "  Daught  " + expObj.srcPlateNo.ToString() + "  " + noOfRepeatedArrayByDaught.ToString() + ">";
                                printLog("[" + System.DateTime.Today + "] >>> " + textLine);
                                tbSystemMsg.Text = tbSystemMsg.Text + "...daught label: " + textLine + '\n';
                                if (isDebug)
                                {
                                    printDebugMsg("^XA");
                                    printDebugMsg("^PQ1,0,1,Y");
                                    printDebugMsg("^XZ");
                                }
                                printText = printText + "^XA^LH0,0^LRN^XZ^XA^FO" + (15 + vDistance) + ",30";
                                strLine1 = "^FT" + (30 + vDistance) + ",30^A0,N,18,30^FD" + expCode.ToUpper().ToString() + "-";
                                strLine1 = strLine1 + expObj.dstPlateNo + "V" + j.ToString() + "^FS";
                                strLine1 = strLine1 + "^FT" + (220 + vDistance) + ",30^A0,N,18,30^FD" + item.multiplex+ "^FS";
                                strLine1 = strLine1 + "^FT" + (280 + vDistance) + ",30^A0,N,18,30^FD" + item.extNo + "^FS";
                                strLine1 = strLine1 + "^FT" + (440 + vDistance) + ",30^A0,N,18,30^FD" + item.arrNo + "^FS";
                                if (isDebug)
                                {
                                    printDebugMsg(strLine1);
                                }
                                printText = printText + strLine1;
                                //write barcode
                                strLine2 = "^FT170,60^BY1^BCN,30,N,N^FD" + expCode.ToUpper().ToString() + "-" + expObj.dstPlateNo + "V" + j.ToString();
                                if (isDebug)
                                {
                                    printDebugMsg(strLine2);
                                    printDebugMsg("^XZ");
                                }
                                printText = printText + strLine2 + "^XZ";
                               
                                printText = printText + "^XA^PQ1,0,1,Y^XZ";
                            }
                        }
                    }
                }
                if ((!isDebug) && (rbPrinter.Checked == true))
                {
                    sendToPrint(printText.ToString(), IPAddress.Parse(smallLabelPrinter));
                    printLog("[" + System.DateTime.Today + "] >>> " + "nexar daughter printing starts");
                    tbSystemMsg.Text = tbSystemMsg.Text + "nexar daughter printing starts..." + '\n';
                }
                else
                {
                    printLog("[" + System.DateTime.Today + "] >>> " + "nexar daughter printing not started");
                    tbSystemMsg.Text = tbSystemMsg.Text + "nexar daughter printing not started!" + '\n';
                }
            }
            else
            {
                if (!isDebug)
                {
                    MessageBox.Show("No daughter data found for printing!");
                }
                else
                {
                    printDebugMsg("No daughter data found for printing!");
                }      
            }


            if ((listDaugs.Count() > 0) && (rbLabeler.Checked == true))
            {
                string folderPath = rootDir + folderName;
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileStr = "\\DaugNexar-" + expCode + ".csv";
                string fileDir = folderPath + fileStr;

                var csvText = new StringBuilder();
                var newLine = string.Format("{0},{1},{2},{3}", "prjName", "multiplex", "extNo", "arrNo");
                csvText.AppendLine(newLine);
                foreach (DaughterItem daugItem in listDaugs)
                {
                    newLine = string.Format("{0},{1},{2},{3}", daugItem.prjName, daugItem.multiplex,daugItem.extNo, daugItem.arrNo);
                    csvText.AppendLine(newLine);
                }
                File.WriteAllText(fileDir, csvText.ToString());
                attachDaugItemsToCSV(listDaugs);
            }
            
        }

        private void printLog(string logText)
        {
            using (FileStream fs = new FileStream(logDir, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    sr.WriteLine(logText);
                }
            }
        }

        private string reforExtStr(string origStr)
        {
            List<string> strArray = new List<string>(origStr.Split(new string[] { "," }, StringSplitOptions.None));
            string newStr = string.Empty;
            foreach(string tempS in strArray)
            {
                newStr = newStr + tempS+"`";
            }
            newStr = newStr.Remove(newStr.Length - 1);
            return newStr;
        }

        private void printDebugMsg(string debugText)
        {
            using (FileStream fs = new FileStream(debugDir, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    sr.WriteLine(debugText);
                }
            }
        }

        private void sendToPrint(string printData,IPAddress HostIPAddress)
        {
            byte[] dataBytes;
            TcpClient printerPort = new TcpClient();
            NetworkStream printedStream;
            try
            {
                if(printData!=null)
                {
                    dataBytes = Encoding.ASCII.GetBytes(printData);
                    if(!printerPort.Client.Connected)
                    {
                        if(isDebug)
                        {
                            printDebugMsg("Not connected and connecting now...");
                        }
                        printLog("[" + System.DateTime.Today + "] >>> "+ "Not connected to "+HostIPAddress.ToString()+ " and connecting now...");
                        printerPort.Connect(HostIPAddress, 9100);
                    }
                    else
                    {
                        if(isDebug)
                        {
                            printDebugMsg("Connected!");                           
                        }
                        printLog("[" + System.DateTime.Today + "] >>> " + "Connected to " + HostIPAddress.ToString());
                    }
                    printedStream = printerPort.GetStream();
                    if(isDebug)
                    {
                        printDebugMsg("DataBytes [" + dataBytes.ToString() + "] and length = [" + dataBytes.Length + "]");
                    }
                    if(printedStream.CanWrite)
                    {
                        printedStream.Write(dataBytes, 0, dataBytes.Length);
                    }
                    printedStream.Close();
                    printerPort.Close();
                }
            }catch(Exception exp)
            {
                printDebugMsg(exp.ToString());
            }
        }

        private void CheckPrinterConnection(IPAddress HostIPAddress)
        {
            TcpClient printerPort = new TcpClient();
            if(printerPort.Client.Connected.Equals(false))
            {
                try
                {
                    printerPort.Connect(HostIPAddress, 9100);
                }catch(Exception exp)
                {
                    exp.ToString();
                    MessageBox.Show("It seems that the connection to the printer " + HostIPAddress.ToString() +
                        "is lost! Please verify that the printer is connected to the network by either "
                        + "re-connecting the network cable to the printer or re-starting the printer "
                        + "prior to print.");
                }
            }
        }

        private void BarcodeGenerator_Load(object sender, EventArgs e)
        {
            this.cbTest.Checked = true;
            this.rbLabeler.Checked = true;
            this.cbExtraction.Checked = true;
            this.cbDaughter.Checked = true;
            this.cbAssay.Checked = true;
            tbSystemMsg.Text = string.Empty;
            this.progressBar.Minimum = 0;
            this.progressBar.Maximum = 100;
            this.progressBar.Value = 0;
            this.progressBar.Step = 10;
            this.CenterToScreen();
        }

        private void rbPrinter_CheckedChanged(object sender, EventArgs e)
        {
            this.cbTest.Enabled = false;
        }

        private void attachExtItemsToCSV(List<ExtractionItem> listExt)
        {
            string fileDir = igFolder + "\\" + tExtFile;
            /* generate csv file */
            if ((listExt.Count() > 0) && (rbLabeler.Checked == true))
            {
                var csvText = new StringBuilder();
                foreach (ExtractionItem extItem in listExt)
                {
                    var newLine = string.Format("{0},{1},{2}", extItem.prjName, extItem.date, extItem.extNo);
                    csvText.AppendLine(newLine);
                }
                File.AppendAllText(fileDir, csvText.ToString());
            }
        }

        private void attachAssayItemsToCSV(List<AssayItem> listAssays)
        {
            string fileDir = igFolder + "\\" + tAssayFile;
            /* generate csv file */
            if ((listAssays.Count() > 0) && (rbLabeler.Checked == true))
            {
                var csvText = new StringBuilder();
                foreach (AssayItem assayItem in listAssays)
                {
                    var newLine = string.Format("{0},{1},{2}", assayItem.prjName, assayItem.assayNo, assayItem.mplxNo);
                    csvText.AppendLine(newLine);
                }
                File.AppendAllText(fileDir, csvText.ToString());
            }
        }

        private void attachDaugItemsToCSV(List<DaughterItem> listDaugs)
        {
            string fileDir = igFolder + "\\" + tDaughtFile;
            /* generate csv file */
            if ((listDaugs.Count() > 0) && (rbLabeler.Checked == true))
            {
                var csvText = new StringBuilder();
                foreach (DaughterItem daugItem in listDaugs)
                {
                    var newLine = string.Format("{0},{1},{2},{3}", daugItem.prjName, daugItem.multiplex, daugItem.extNo, daugItem.arrNo);
                    csvText.AppendLine(newLine);
                }
                File.AppendAllText(fileDir, csvText.ToString());
            }        
        }

        private void rbLabeler_CheckedChanged(object sender, EventArgs e)
        {
            this.cbTest.Enabled = true;
        }

    }
}
