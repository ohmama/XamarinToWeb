using System;
namespace WebController
{
    public class Utils
    {
        public Utils()
        {
        }
        public static bool IsNumeric(string Expression)
		{
			//double retNum;
			//bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            int output;
            return int.TryParse(Expression, out output);
		}

        public static bool IsValidUrl(string url){
			Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult) && Uri.IsWellFormedUriString(url,UriKind.RelativeOrAbsolute);
        }
    }
}
