using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace ReIncarnation_Quest_Maker.Obsidius
{
    public static class Unbidden
    {
        public static void Write(string FilePath, string Data) {
            StreamWriter TextFile = File.CreateText(FilePath);
            TextFile.WriteLine(Data);
            TextFile.Close();
        }
    }
}
