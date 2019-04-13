using System;

namespace Hladnjaca
{
    class KorisnickiException : Exception
    {
        public KorisnickiException() : base() { }
        public KorisnickiException(string poruka) : base(poruka) { }
        public KorisnickiException(string poruka, Exception inner) : base(poruka, inner) { }
    }
}
