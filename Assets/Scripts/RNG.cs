using System;

public class RNG
{
	private static Random RandomNumberGenerator = new Random();

	private RNG () {}

	public static float NextFloat()
	{
		return (float)RandomNumberGenerator.NextDouble ();
	}

	public static float NextFloat(int min, int max)
	{	
		return (float)RandomNumberGenerator.NextDouble() + RandomNumberGenerator.Next(min, max-1);
	}

	public static int Next()
	{
		return RandomNumberGenerator.Next();
	}

	public static int Next(int min, int max)
	{
		return RandomNumberGenerator.Next(min, max);
	}


	public static double NextDouble()
	{
		return RandomNumberGenerator.NextDouble();
	}

	public static bool RandomBoolean()
	{
		if ((RandomNumberGenerator.Next () % 2) == 0) {
			return false;
		}
		return true;
	}
}