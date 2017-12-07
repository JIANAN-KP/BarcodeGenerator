using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BarcodeGenerator
{
    public static class ExtFunctions
    {
        public static List<FileInfo> SortFiles(this List<FileInfo> list)
        {
            List<FileInfo> newList = new List<FileInfo>();
            foreach(FileInfo fileInfo in list)
            {
                int position = 0;
                int start = fileInfo.Name.IndexOf("-") + 1;
                int length = (fileInfo.Name.IndexOf("P") - (fileInfo.Name.IndexOf("-")+1));
                int distCode = Convert.ToInt32(fileInfo.Name.Substring(fileInfo.Name.IndexOf("-") + 1, (fileInfo.Name.IndexOf("PV") - (fileInfo.Name.IndexOf("-")+1))));
                int repCode = Convert.ToInt32(fileInfo.Name.Substring(fileInfo.Name.IndexOf("V")+1,1));
                for(int m =0;m<newList.Count();m++)
                {
                    FileInfo fInfo = newList[m];
                    int ndistCode = Convert.ToInt32(fInfo.Name.Substring(fInfo.Name.IndexOf("-") + 1, (fInfo.Name.IndexOf("PV") - (fInfo.Name.IndexOf("-") + 1))));
                    int nrepCode = Convert.ToInt32(fInfo.Name.Substring(fInfo.Name.IndexOf("V") + 1, 1));
                    if(ndistCode<distCode)
                    {
                        position = m;
                        position = position + 1;               
                    }
                    else
                    {
                        if(ndistCode>distCode)
                        {
                            position = m;
                            break;
                        }
                        else //ndistCode=distCode
                        {
                            if(nrepCode>repCode)  //either larger or smaller
                            {
                                position = m;
                                break;
                            }else
                            {
                                position = m;
                                position = position + 1;
                            }
                        }
                    }
                }
                newList.Insert(position, fileInfo);
            }
            return newList;
        }
    }
}
