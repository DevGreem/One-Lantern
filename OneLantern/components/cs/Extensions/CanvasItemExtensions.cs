using Godot;
using Godot.Collections;

public static partial class CanvasItemExtensions
{
	
	public static bool CanReach(this CanvasItem obj, CanvasItem target, Array<Rid> exclude, uint collisionMask = 0)
	{
		PhysicsDirectSpaceState2D space = obj.GetWorld2D().DirectSpaceState;

		var query = PhysicsRayQueryParameters2D.Create(
			obj.GetGlobalTransform().Origin,
			target.GetGlobalTransform().Origin
		);

		query.Exclude = exclude;
		query.CollisionMask = collisionMask;
		
		var result = space.IntersectRay(query);

		if (result.Count == 0)
			return true;
		
		return result["collider"].As<CanvasItem>() == target;
	}
}