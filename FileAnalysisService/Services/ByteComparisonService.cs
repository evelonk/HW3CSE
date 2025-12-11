namespace FileAnalysisService.Services;

public class ByteComparisonService
{
    public double Compare(byte[] a, byte[] b)
    {
        var minLength = Math.Min(a.Length, b.Length);
        var maxLength = Math.Max(a.Length, b.Length);

        if (maxLength == 0)
            return 100;

        int n = 0;

        for (int i = 0; i < minLength; i++)
        {
            if (a[i] == b[i])
                n++;
        }

        return (double)n / maxLength * 100.0;
    }
}
