using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionTestApp
{

    class Program
    {
        static void Main(string[] args)
        {
            int x = 100, y = 5, value = 0;



            // 에러 try catch 구문.
            try
            {
                value = x / y;
                Console.WriteLine($"{x} / {y} = {value}"); // 정상적으로 실행 되는 곳
               // throw new Exception("1 사용자 에러"); // 정상적으로 되어도 사용자 정의로 그냥 에러를 띄움
            }
            catch (DivideByZeroException ex) {

                Console.WriteLine("2. y 의 값을 0보다 크게 입력하세요");
            }
            catch (Exception ex) //exception는 모든 에러의 부모이다. 상세한 에러 잡는 것 보다 맨 마지막에 있어야 한다.
            {
                /*Console.WriteLine(ex.ToString());  --에러 시 에러 위치 등 자세하게 출력*/
                Console.WriteLine("3" + ex.Message); // 에러 시 위치 등 나오지 않고 메시지만 출력
                
            }
            finally
            {
                Console.WriteLine("4. 프로그램이 종료하였습니다"); // finally에서는 에러가 생기던 안생기던 무조건 실행 되는 곳이다.
            }
        }
    }
}
