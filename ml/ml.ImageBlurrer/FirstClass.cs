using System;
using System.Collections.Generic;
using Numpy;

namespace ml.ImageBlurrer
{
    public class FirstClass
    {
        public void Make()
        {
            //numpy

            var x = 3.721;
            
            //random
            var y = np.random.rand();
            Console.WriteLine(np.random.randint(13, 34));
            Console.WriteLine(np.random.rand(5,5));
            
            //array
            Console.WriteLine(np.zeros(3, 2));
            Console.WriteLine(np.linspace(2, 50, 5));
            Console.WriteLine(np.empty(0, 3));

            var list = new List<int>
            {
                5, 6, 7
            };

            var npList = np.array<int>(list.ToArray());
            Console.WriteLine(npList);

            var A = np.random.rand(2, 4);
            Console.WriteLine(A);
            var B = np.ones(4, 3);
            Console.WriteLine(B);
            var C = 2 * B;
            Console.WriteLine(C);

            Console.WriteLine(B * C);

            Console.WriteLine(np.dot(B.T, C));


            var yMatrix = np.random.rand(7, 6);
            Console.WriteLine(yMatrix);
            Console.WriteLine(yMatrix[3, 5]);

            Console.WriteLine(np.sum(yMatrix));
            Console.WriteLine(yMatrix.argmax(1));
            
            //matplotlib
            //var matplotlib = new MatplotlibCS.MatplotlibCS(pythonExePath, matplotlibPyPath);
        }
    }
}