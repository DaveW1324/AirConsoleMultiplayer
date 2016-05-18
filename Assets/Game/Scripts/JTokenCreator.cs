using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;

public class JTokenCreator
{

    /// <summary>
    /// Simulate the message created by moving the Joystick 
    ///
    /// Correct format:
    /// {
    ///     "joystick-left": 
    ///     {
    ///         "pressed": true,
    ///         "message":
    ///         {
    ///             "x": -0.85269799658066181,
    ///             "y": 0.42240417937390762
    ///         }
    ///     }
    /// }
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static JToken GetControllerJToken(Vector2 direction)
    {
        return new JObject(
                new JProperty("joystick-left", new JObject(
                    new JProperty("pressed", true),
                    new JProperty("message",  new JObject(
                        new JProperty("x", direction.x),
                        new JProperty("y", direction.y))))));
    }


    /// <summary>
    /// Simulate the message created by pressing the Shoot button
    /// Correct Format: 
    /// {
    ///     "Shoot": 
    ///     { 
    ///         "pressed": true, 
    ///         "message": {} 
    ///     }
    ///  }
    /// </summary>
    /// <returns></returns>
    public static JToken GetShootJToken()
    {
        return new JObject(
                new JProperty("Shoot", new JObject(
                    new JProperty("pressed", true),
                    new JProperty("message", null))));
    }
}


