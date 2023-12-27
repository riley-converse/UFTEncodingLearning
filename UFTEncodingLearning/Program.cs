using System.ComponentModel.Design.Serialization;
using System.Text;

/**
 * Riley Converse
 * Created this to better understand the different encodings, their uses, and how to interact with their values. 
 * 
 */
namespace UFTEncodingLearning
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /** Unicode(UTF/ Unicode Transformation Format) characters can be translated into hexadecimal, decimal, or binary, and back again.
             * UTF-8, UTF-16, and UTF-32 are encodings of Unicode. The decimal indicates the required
             * bits needed for a single character.
             * UTF-8 is  8  bits or 1 byte for one character
             * UTF-16 is 16 bits or 2 bytes for two characters.
             * 
             * The benefits or prefered encoding is based on the expected text.
             * 
             * UTF-8 
             * Pros:
             * 1) Is best when the majority of the text consists of ascii values as this will results in 1 byte characters.
             * 2) Requires no designation of endian due to the encoding process.
             * 3) Backwards compatidable with ASCII
             * Cons:
             * 1) If text uses characters outside of ascii, can results in character length being 2,3, or 4 bytes, depending on the character.
             * 
             * --------------------------------------
             * 
             * UTF-16 
             * Pros:
             * 1) Characters size can be smaller then UTF-8 if dealing with characters outside of ascii. For example,  
             * the € charachter is 3 bytes long due to UTF-8 encoding, while only 2 bytes in UTF-16
             * 
             * Cons:
             * 1) If dealing with only ASCII values, then it requires, at mininum, double the number of bytes then UTF-8. (min is 2 or 4 bytes).
             * 2) Requires endian designation. Little endian or bigendian will affect the value. 
             * 
             * The reason for difference in byte size is because each encoding uses a variable length. Some characters require additional values
             * which requires additional bits, which means additional byte(s). In UTF-8, the character size is 8 bits at mininum. But, if needing to use
             * a character from the extended table, it needs additional bits, which results in another byte. Simply put, 8 bits = 1 byte = 256 potential
             * combinations which is not enough to encode all human language. Therefore using additional bytes along with variable length encoding extends
             * capable encoding to over a million. 
             * 
             * Notes, UTF-8 and UTF-16 are both part of unicode. But C# refers to UTF-16 as unicode. 
            */

            // alows conssole to print out in encoding greater then ascii. 
            Console.OutputEncoding = Encoding.UTF8;

            // Standard char is 2 bytes because it's Unicode(UTF-16) by default. 2 Bytes.
            Console.WriteLine("Size of Unicode Char: " + sizeof(char) + " bytes.");

            // Can write literal obviously.
            Console.Write($"\nThe Unicode literal of char 'M' is 'M'. Example ->");
            Console.Write('M');
           
            // Can write as unicode using \u (recommmended over \x)/ 
            // Note, that the byte stores a decimal which is converted to hexadecimal. Hexadecimal
            // is the value that's usually references in the table lookup. 
            Console.Write($"\nThe unicode sequence of char 'M' is '004D'. Example ->");
            Console.Write($"\x004D");

            // Can write as hexadecimal using \x (more error prone).
            Console.Write("\nThe hexadecimal sequence of char 'M' is '004D'. Example ->");
            Console.Write('\x004D');

            Console.WriteLine();
            // ascii shows the same value as UTF-8 since it's reverse compadiable
            PrintASCII('b');
            PrintHex('b', 7);

            PrintUTF8('b');
            PrintHex('b', 8);

            PrintUTF16('b');
            PrintHex('b',16);
            Console.WriteLine();

            PrintUTF8('z');

            Console.WriteLine();
            PrintUTF8('£');
            PrintHex('£', 8);

            PrintUTF16('£');          
            PrintHex('£', 16);
            Console.WriteLine();

            Console.WriteLine();
            // Ascii shows the incorrect value (shows as ?) because
            // this tables does not contain a definition. 
            PrintASCII('€');
            PrintHex('€', 7);

            PrintUTF8('€');
            PrintHex('€');

            PrintUTF16('€');        
            PrintHex('€',16);
            Console.WriteLine();

            Console.WriteLine();
            PrintUTF8('한');
            PrintHex('한');

            PrintUTF16('한');
            PrintHex('한', 16);
            Console.WriteLine();
        }

        static byte[] PrintASCII(char c, bool print = true)
        {
            Encoding encoder = Encoding.ASCII;
            char[] charArray = new char[] { c };

            byte[] arrayOfBytes = new byte[4];
            arrayOfBytes = encoder.GetBytes(charArray);

            if (print)
            {
                Console.Write($"\nThe letter [{c}] ASCII in decimal by bytes: \t");
                foreach (byte b in arrayOfBytes) { Console.Write("["); Console.Write(b); Console.Write("] "); }
            }

            return arrayOfBytes;
        }

        static byte[] PrintUTF8(char c, bool print=true)
        {
            Encoding encoder = Encoding.UTF8;
            char[] charArray = new char[] { c };

            byte[] arrayOfBytes = new byte[4];
            arrayOfBytes = encoder.GetBytes(charArray);

            if (print)
            {
                Console.Write($"\nThe letter [{c}] UTF-8 in decimal by bytes: \t");
                foreach (byte b in arrayOfBytes) { Console.Write("["); Console.Write(b); Console.Write("] "); }
            }

            return arrayOfBytes;
        }
        static byte[] PrintUTF16(char c, bool print=true)
        {
            Encoding encoder = Encoding.BigEndianUnicode;
            char[] charArray = new char[] { c };

            byte[] arrayOfBytes = new byte[4];
            arrayOfBytes = encoder.GetBytes(charArray);
             if (print)
            {
                Console.Write($"\nThe letter [{c}] UTF-16 in in decimal by bytes: \t");
                foreach (byte b in arrayOfBytes) { Console.Write("["); Console.Write(b); Console.Write("] "); }
                
            }

            return arrayOfBytes; 
        }

        static void PrintHex(char c,int UTFType=8)
        {
            byte[] arrayOfBytes;
            if (UTFType == 0 || UTFType == 8)
            {
                arrayOfBytes = PrintUTF8(c, false);
            }
            else if (UTFType == 1 || UTFType == 16)
            {
                arrayOfBytes = PrintUTF16(c, false);
            }
            else if (UTFType == 3 || UTFType == 7)
            {
                arrayOfBytes = PrintASCII(c, false);
            }
            else
            {
                Console.WriteLine($"No UTFType found for {UTFType}");
                return;
            }

            Console.Write($"\nThe letter [{c}] UTF-{UTFType} in hexadecimal by bytes: \t");
            foreach (byte b in arrayOfBytes ) 
            {
                Console.Write("[");
                PrintHex(b);
                Console.Write("] ");
            }
        }

        static void PrintHex(decimal dec)
        {
            Dictionary<decimal, string> hexConversion = new Dictionary<decimal, string>();
            hexConversion.Add(0, "0");
            hexConversion.Add(1, "1");
            hexConversion.Add(2, "2");
            hexConversion.Add(3, "3");
            hexConversion.Add(4, "4");
            hexConversion.Add(5, "5");
            hexConversion.Add(6, "6");
            hexConversion.Add(7, "7");
            hexConversion.Add(8, "8");
            hexConversion.Add(9, "9");
            hexConversion.Add(10, "A");
            hexConversion.Add(11, "B");
            hexConversion.Add(12, "C");
            hexConversion.Add(13, "D");
            hexConversion.Add(14, "E");
            hexConversion.Add(15, "F");

            string hex = "";
            MidpointRounding roundDown = MidpointRounding.ToZero;
            decimal remainder = 1;
            decimal whole = 0;
        
            if (dec % 16 == 0 && dec > 15)
            {
                int zeroPlacement;
                (dec, zeroPlacement) = HandleRoot(dec, 16);

                hex += hexConversion[dec];
                for (int i = 0; i<zeroPlacement; i++)
                {
                    hex += "0";
                }
            }
            else
            {
                // we assume we have a decimal and it's under 16. We finish when we have a whole number again.
                while (remainder > 0)
                {
                    if (dec > 15)
                    {
                        dec /= 16;
                    }
                    else
                    {
                        whole = Math.Round(dec, roundDown);
                        remainder = dec - whole;
                        hex += hexConversion[whole];
                        dec = remainder *= 16;
                    }
                }
            }

            Console.Write($"{hex}");

        }

        static (decimal, int) HandleRoot(decimal dec, decimal baseValue, int attemptLimit=10)
        {
            int loopCounter = 0;
            while (dec >= baseValue)
            {
                if (loopCounter >= attemptLimit)
                    return (-1, -1);

                dec /= baseValue;
                loopCounter++;
            }
            return (dec, loopCounter);
        }
    }

   
}
