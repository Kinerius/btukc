using System;

namespace Utils
{
    public static class CodeUtils
    {
        public static void RecursiveOperation(Func<bool> operation, int bailOut, Action onFailed = null)
        {
            var i = 0;
            while (true)
            {
                if (i >= bailOut)
                {
                    onFailed?.Invoke();
                    return;
                }

                if (!operation())
                {
                    i++;

                    continue;
                }

                break;
            }
        }
    }
}