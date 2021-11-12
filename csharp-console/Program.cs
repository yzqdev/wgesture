using System;

namespace csharp_console {
    class Program   {
         
        static void Main(string[] args)
        {
            var text = "";
            ProgramBase programBase = new ProgramBase();
            ///显示Environment.SpecialFolder
            string[] enumArr = Enum.GetNames(typeof(Environment.SpecialFolder));
            Array enumValueArr = Enum.GetValues(typeof(Environment.SpecialFolder));
            for (int i = 0, j = 1; i < enumArr.Length; i++, j++)
            {
                text += j.ToString() + ":" +programBase.MakeSpace(j.ToString(), 10) + enumArr[i] + programBase.MakeSpace(enumArr[i], 30) + Environment.GetFolderPath((Environment.SpecialFolder)enumValueArr.GetValue(i)) + "\r\n";
                Console.WriteLine(text);
            }
            /////空格对齐
        }
    }
}
