using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PropertyPlus.Helper
{
    public static class URL
    {
        public static string ToRewriteUrl(this string url)
        {
            string encodedUrl = (url ?? "").ToLower();
            encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");
            encodedUrl = encodedUrl.Replace("'", "");
            encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");
            encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");
            encodedUrl = encodedUrl.Trim('-');
            return encodedUrl;
        }

        public static String ToAscii(this String unicode)
        {
            unicode = unicode ?? "";
            unicode = Regex.Replace(unicode.ToLower(), "[áàảãạăắằẳẵặâấầẩẫậ]", "a");
            unicode = Regex.Replace(unicode.ToLower(), "[óòỏõọôồốổỗộơớờởỡợ]", "o");
            unicode = Regex.Replace(unicode.ToLower(), "[éèẻẽẹêếềểễệ]", "e");
            unicode = Regex.Replace(unicode.ToLower(), "[íìỉĩị]", "i");
            unicode = Regex.Replace(unicode.ToLower(), "[úùủũụưứừửữự]", "u");
            unicode = Regex.Replace(unicode.ToLower(), "[ýỳỷỹỵ]", "y");
            unicode = Regex.Replace(unicode.ToLower(), "[đ]", "d");
            unicode = Regex.Replace(unicode, "[-\\s+/]+", "-");
            unicode = Regex.Replace(unicode, "\\W+", " "); //Nếu bạn muốn thay dấu khoảng trắng thành dấu "_" hoặc dấu cách " " thì thay kí tự bạn muốn vào đấu "-"
            return unicode;
        }
    }
}