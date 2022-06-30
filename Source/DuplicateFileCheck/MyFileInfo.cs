using System;
using System.Collections.Generic;
using System.Text;

namespace DEVGIS.DuplicateFileCheck
{
    public class MyFileInfo
    {
        public string Key
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public List<MyFileInfo> DuplicateFiles
        {
            get;
            set;
        }
    }
}
