using System;

namespace Al.Security.Pkix
{
    public class PkixNameConstraintValidatorException : Exception
    {
        public PkixNameConstraintValidatorException(String msg)
            : base(msg)
        {
        }
    }
}
