/*
 * A class made to help tracking the combatants for Turn Manager
 */


public class CombatantTracker
{
    public enum Identifier {ALLY1, ALLY2, ALLY3, ENEMY1, ENEMY2, ENEMY3 };

    private int order;
    private Identifier identifier;
    private Combatant script;

	public int Order { get => order; set => order = value; }
	public Identifier GetIdentifier()
	{
		return identifier;
	}
    private void SetIdentifier(bool isAlly, int number)
	{
		if (isAlly)
		{
			switch (number)
			{
				case 1:
					identifier = Identifier.ALLY1;
					break;
				case 2:
					identifier = Identifier.ALLY2;
					break;
				case 3:
					identifier = Identifier.ALLY3;
					break;
			}
		}
		else
		{
			switch (number)
			{
				case 1:
					identifier = Identifier.ENEMY1;
					break;
				case 2:
					identifier = Identifier.ENEMY2;
					break;
				case 3:
					identifier = Identifier.ENEMY3;
					break;
			}
		}
	}
	public Combatant Script { get => script; set => script = value; }

	public void Add(int orderNo, bool isAlly, int charNumber, Combatant combatantScript)
	{
		order = orderNo;
		SetIdentifier(isAlly, charNumber);
		script = combatantScript;
	}
}
