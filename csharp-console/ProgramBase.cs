namespace csharp_console {
    public  class ProgramBase {
      public  string MakeSpace(string str, int len)
        {
            string rst = string.Empty;
            int sum = len - str.Length;
            if (sum < 1) return rst;
            for (int i = 0; i < sum; i++)
            {
                rst += " ";
            }
            return rst;
        }
    }
}