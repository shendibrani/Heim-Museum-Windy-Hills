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
	float tOne, tTwo, tThree;

	public TimedGrade(float thresholdOne,float thresholdTwo, float thresholdThree)
	{
		//this.cut = cut;
		tOne = thresholdOne;
		tTwo = thresholdTwo;
		tThree = thresholdThree;
	}

	public override bool OnePointGrade ()
	{
		return true; //TutorialProgression.Instance.timer < tOne;
	}

	public override bool TwoPointGrade ()
	{
		return true; //Tcut.timer < tTwo;
	}

	public override bool ThreePointGrade ()
	{
		return true; //Tcut.timer < tThree;
	}
}

public class FeedbackGrade : GradingParameters
{
	bool step1, step2, step3;

	public FeedbackGrade(bool s1, bool s2, bool s3)
	{
		step1 = s1;
		step2 = s2;
		step3 = s3;
	}

	public override bool OnePointGrade ()
	{
		//One point condition
		return step1;
	}

	public override bool TwoPointGrade ()
	{
		//Two point condition
		return step2;
	}

	public override bool ThreePointGrade ()
	{
		//Three point condition
		return step3;
	}
}