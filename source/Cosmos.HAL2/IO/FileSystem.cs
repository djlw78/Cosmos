using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.HAL2.IO
{
    public class FileStream : IOStream
    {
        private string FileName = "";
        private string FileMode = "";

        public FileStream(string fname, string mode)
        {
            this.FileName = fname;
            this.init(7000);
            this.FileMode = mode;
            if (!(mode == "r"))
                return;
            this.init(FileSystem.Root.readFile(fname).Length);
            this.Data = FileSystem.Root.readFile(fname);
        }

        public override void Flush()
        {
            base.Flush();
        }

        public override void Write(byte @byte)
        {
            base.Write(@byte);
        }

        public override byte Read()
        {
            return base.Read();
        }

        public override void Close()
        {
            if (!(this.FileMode == "w"))
                return;
            MemoryStream memoryStream = new MemoryStream(this.Position);
            for (int index = 0; index < this.Position; ++index)
                memoryStream.Write(this.Data[index]);
            this.Data = memoryStream.Data;
            FileSystem.Root.saveFile(this.Data, this.FileName, CurrentUser.Username);
        }
    }
}
