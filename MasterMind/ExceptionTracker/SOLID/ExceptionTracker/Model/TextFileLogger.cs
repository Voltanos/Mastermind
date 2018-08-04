using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionTracker.SOLID.ExceptionTracker.Model
{
    public class TextFileLogger
    {
        #region Member Variables

        private string Path;
        private string FileName;
        private string PathAndFileName;

        #endregion

        #region Constructor

        public TextFileLogger(string path, string fileName)
        {
            this.Path = path;
            this.FileName = fileName;

            CreateDirectoryAndFile();
        }

        #endregion

        #region Public Methods

        public void Log(string message)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.PathAndFileName, true))
            {
                file.WriteLine(ApplyTimeStampToString(message));
            }
        }

        public void ExceptionLog(string message, string stackTrace)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.PathAndFileName, true))
            {
                file.WriteLine(ApplyTimeStampToString(message));
                file.WriteLine(ApplyTimeStampToString(stackTrace));
            }
        }

        #endregion

        #region Private Methods

        #region Constructor methods

        private void CreateDirectoryAndFile()
        {
            if (System.IO.Directory.Exists(this.Path) == false)
            {
                System.IO.Directory.CreateDirectory(this.Path);
            }            

            this.PathAndFileName = System.IO.Path.Combine(this.Path, this.FileName);

            if (System.IO.File.Exists(this.PathAndFileName) == false)
            {
                System.IO.File.Create(this.PathAndFileName);
            }            
        }

        #endregion

        #region "Log" methods

        private string ApplyTimeStampToString(string message)
        {
            ////////////////////////////////////////////
            //Local Variables

            string returnMessage;

            ////////////////////////////////////////////

            returnMessage = String.Format("{0:MM/dd/yy H:mm:ss}:  {1}", DateTime.Now, message);

            return returnMessage;
        }

        #endregion

        #endregion
    }
}
