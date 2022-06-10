using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace QuayThuoc.Models
{
    public class DungChung
    {
        public String CreateMaHD(String MaHD,int number) {
            if(number < 10)
            {
                MaHD = MaHD + "0000000" + number.ToString();
            }else if (number < 100)
            {
                MaHD = MaHD + "000000" + number.ToString();
            }else if (number < 10000)
            {
                MaHD = MaHD + "00000" + number.ToString();
            }else if (number < 100000)
            {
                MaHD = MaHD + "0000" + number.ToString();
            }
            else{
                MaHD = MaHD + "000" + number.ToString();
            }

            return MaHD;
        }
        public string EncodePassword(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }

    }
}