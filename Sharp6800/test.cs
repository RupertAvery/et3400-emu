using System;

void Main()
{
	for(byte A = 0; A <= 255; A++)
	{
		var a = 0xFF - A;
		a = a & 0x0E;
		a = a >> 1;
		if(a == 0)
		{
		    Console.WriteLine(A.ToString("X4"));
		}
	}
}

