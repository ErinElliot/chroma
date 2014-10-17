using UnityEngine;
public struct Vector3Int
{
	public int x;
	public int y;
	public int z;

	public Vector3Int (int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public static explicit operator Vector3(Vector3Int v)
	{
		return new Vector3 ((float)v.x, (float)v.y, (float)v.z);
	}

	public override int GetHashCode ()
	{
		unchecked // Overflow is fine, just wrap
		{
			int hash = 17;
			// Suitable nullity checks etc, of course :)
			hash = hash * 486187739  + x.GetHashCode();
			hash = hash * 486187739  + y.GetHashCode();
			hash = hash * 486187739  + z.GetHashCode();
			return hash;
		}
	}

	public override bool Equals(System.Object obj)
	{
		// If parameter cannot be cast to Vector3Int return false:
		if (!(obj is Vector3Int))
			return false;
		Vector3Int p = (Vector3Int) obj;
		
		// Return true if the fields match:
		return p.x == x && p.y == y && p.z == z;
	}

	public static bool operator ==(Vector3Int a, Vector3Int b)
	{
		// Return true if the fields match:
		return a.x == b.x && a.y == b.y && a.z == b.z;
	}
	
	public static bool operator !=(Vector3Int a, Vector3Int b)
	{
		return !(a == b);
	}

	public override string ToString ()
	{
		return "("+x+","+y+","+z+")";
	}
}