using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTestingTriangle
{
    public  class TriangleUtility
    {
        public static string GetTriangleType(float sideA, float sideB, float sideC)
        {
            if (sideA + sideB > sideC && sideA + sideC > sideB && sideB + sideC > sideA)
            {
                if (sideA == sideB && sideB == sideC)
                {
                    return "равносторонний";
                }
                else if (sideA == sideB || sideA == sideC || sideB == sideC)
                {
                    return "равнобедренный";
                }
                else
                {
                    return "разносторонний";
                }
            }
            else
            {
                return "не треугольник";
            }
        }
    }
}
