using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBotConsole
{
    public class NetworkType
    {
        public string fileName { get; } //yeah yeah yeah This dosen't belong here
        //public int [] layerSize { get; } //TODO Add later

        public NetworkType(string fileName/*, int[] layerSize*/)
        {
            this.fileName = fileName; 
            //this.layerSize = layerSize;
        }
    }


    public static class GenTypes //This dosen't feel right.
    {
        public static NetworkType First { get; } = new NetworkType("");
        public static NetworkType ReLU { get; } = new NetworkType("ReLU");
    }
}
