using System;
using UnityEngine;

public abstract class GradingParameters
{
	public int score { get; private set; }

	public GradingParameters () {}

	public abstract bool OnePointGrade();
	public abstract bool TwoPointGrade();
	public abstract bool ThreePointGrade();

	public int CalculateScore(){
		score = 0;
		if (OnePointGrade ()) {
			score = 1;
			if (TwoPointGrade ()) {
				score = 2;
				if (ThreePointGrade ()) {
					score = 3;
				}
			}
		}
		return score;
	}

}
	
public class TimedGrade : GradingParameters
{
	Cutscene cut;

	float tOne, tTwo, tThree;

	public TimedGrade(Cutscene cut, float thresholdOne,float thresholdTwo, float thresholdThree)
	{
		this.cut = cut;
		tOne = thresholdOne;
		tTwo = thresholdTwo;
		tThree = thresholdThree;
	}

	public override bool OnePointGrade ()
	{
		return cut.timer < tOne;
	}

	public override bool TwoPointGrade ()
	{
		return cut.timer < tTwo;
	}

	public override bool ThreePointGrade ()
	{
		return cut.timer < tThree;
	}
}

public class FeedbackGrade : GradingParameters
{


	public FeedbackGrade()
	{

	}

	public override bool OnePointGrade ()
	{
		//One point condition
	}

	public override bool TwoPointGrade ()
	{
		//Two point condition
	}

	public override bool ThreePointGrade ()
	{
		//Three point condition
	}
}