using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWhitAuth.Interfaces
{
    public interface IChatRepository
    {
        void Add(string name, string message);
    }
}
