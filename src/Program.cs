using System.Drawing;
using System.Drawing.Imaging;
class Demo {
    static int Main(string[] args) {
        if (args.Length != 2) {
            System.Console.WriteLine("Usage: PROGRAM input output ");
            return -1;
        }
        string inname  = args[0];
        string outname = args[1];
        using ( Bitmap bitmap = new Bitmap(inname) ) {
            Process(bitmap);
            bitmap.Save(outname,ImageFormat.Jpeg);
            return 0;
        }
    }

    private static void Process(Bitmap bitmap) {
        if (bitmap.PixelFormat != PixelFormat.Format24bppRgb && bitmap.PixelFormat != PixelFormat.Format32bppArgb) { // <== A1
            return;
        }
        int ww = bitmap.Width  / 8;
        int hh = bitmap.Height / 8;

        using (FastBitmap fbitmap = new FastBitmap(bitmap, ww, hh, bitmap.Width - 2 * ww, bitmap.Height - 2 * hh)) { // <== A2
            unsafe {                                                                                                 // <== A3
                byte* row = (byte*)fbitmap.Scan0, bb = row;                                                          // <== A4
                for (    int yy = 0; yy < fbitmap.Height; yy++, bb  = (row += fbitmap.Stride)) {                     // <== A5
                    for (int xx = 0; xx < fbitmap.Width ; xx++, bb += fbitmap.PixelSize) {                           // <== A6
                        // *(bb + 0) is B (Blue ) component of the pixel
                        // *(bb + 1) is G (Green) component of the pixel
                        // *(bb + 2) is R (Red  ) component of the pixel
                        // *(bb + 3) is A (Alpha) component of the pixel ( for 32bpp )
                        byte gray = (byte)((1140 * *(bb + 0) + 5870 * *(bb + 1) + 2989 *  *(bb + 2)) / 10000);        // <== A7
                        *(bb + 0) = *(bb + 1) = *(bb + 2) = gray;
                    }
                }
            }
        }
    }
}

