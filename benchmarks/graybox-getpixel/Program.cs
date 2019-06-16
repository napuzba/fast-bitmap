using System.Drawing;
using System.Drawing.Imaging;
class Demo {
    static int Main(string[] args) {
        if (args.Length != 2) {
            System.Console.WriteLine("Usage: PROGRAM input output ");
            return -1;
        }
        string inname = args[0];
        string outname = args[1];
        using (Bitmap bitmap = new Bitmap(inname)) {
            Process(bitmap);
            bitmap.Save(outname, ImageFormat.Jpeg);
            return 0;
        }
    }

    private static void Process(Bitmap bitmap) {
        if (bitmap.PixelFormat != PixelFormat.Format24bppRgb && bitmap.PixelFormat != PixelFormat.Format32bppArgb) { // <== A1
            return;
        }
        int w0 = bitmap.Width / 8;
        int h0 = bitmap.Height / 8;
        int x1 = w0;
        int y1 = h0;
        int xn = x1 + bitmap.Width - 2 * w0;
        int yn = y1 + bitmap.Height - 2 * h0;

        Color gray, cc;
        for (    int yy = y1; yy < yn; yy++) {
            for (int xx = x1; xx < xn; xx++) {
                cc = bitmap.GetPixel(xx, yy);
                byte gg = (byte)((cc.B * 1140 + cc.G * 5870 + cc.R * 2989) / 10000);
                gray = Color.FromArgb( gg,gg,gg);
                bitmap.SetPixel(xx, yy, gray);
            }
        }
    }
}

