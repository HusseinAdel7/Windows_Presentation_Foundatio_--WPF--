using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NoteLock
{
    class Validation
    {
        string _passs;
        #region Full Name Validation
        public bool FullName(string fna)
        {
            bool flage = false;
            string pattern = "^[A-Za-z]*$";
            if (Regex.IsMatch(fna, pattern))
            {
                if (!(fna == string.Empty || fna.Length <= 3))
                {
                    if (fna.Length > 3)
                    {
                        flage = true;
                    }
                }
            }
            if (flage == true) { return true; }
            else { return false; }
        }
        public string FullNameError(string fnamee)
        {
            bool vali = FullName(fnamee);
            if (!vali)
            {
                return "Full Name Not Valid";
            }
            else
            {
                return "";
            }
        }
       
        #endregion


        #region Email Validation
        public bool Email(string emaill)
        {
            bool flage = false;
            string pattern = @"^[A-Za-z0-9._%+-]+@gmail\.com$";
            if (Regex.IsMatch(emaill, pattern))
            {
                if (!(emaill == string.Empty))
                {
                    flage = true;
                }
            }
            if (flage == true) { return true; }
            else { return false; }
        }
        public string EmailError(string emailll)
        {
            bool vali = Email(emailll);
            if (!vali)
            {
                return "Email Not Valid";
            }
            else
            {
                return "";
            }
        }
        #endregion


        #region Password and Confirmation Password Validation
        public bool Password(string pass)
        {
            _passs = pass;
            bool flage = false;

            if (pass.Length >= 8)
            {
                flage = true;
            }
            if (flage == true) { return true; }
            else { return false; }
        }
        public string passwoedError(string pass)
        {
            bool vali = Password(pass);
            if (!vali)
            {
                return "Password Not Valid";
            }
            else
            {
                return "";
            }
        }
        public string passwoedConfirmationError(string pass, string pass2)
        {
            bool vali = Password(pass);
            if (pass2 == pass && !(pass.Length < 8))
            {
                return "";
            }
            else
            {
                return "Password con Not Valid";
            }
        }
        #endregion

    }
}
