using UnityEngine;

public static class CowsCompendium
{
    public static Vector3 GetMousePosition(Camera cameraReference, Vector2 mouseInputPosition, Transform objectTransform) // function to grab the location of the mouse
    {
        // Get the world point of the mouse from the screen mouse and then get the location to that from the object transform
        var mousePosition = cameraReference.ScreenToWorldPoint(new Vector3(mouseInputPosition.x, mouseInputPosition.y, cameraReference.transform.position.z * -1));
        mousePosition = objectTransform.InverseTransformPoint(mousePosition);

        // return that mouse position vector
        return mousePosition;
    }
        
    public static float GetAngleFromVectorFloat(Vector3 direction) // A function to get an angle from a vector value "direction"
    {
        // Normalize the direction that was passed and call a new variable Atan from the direction X and Y and multiply it by a radians to degree math function
        direction = direction.normalized;
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // If "n" is less than 0 then add 360 degrees since a circle is 0-360 degrees
        if (n < 0)
            n += 360;

        // Return the value that was translated from a vector into an angle
        return n;
    }
}