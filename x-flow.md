# Example 1_1

1. In this code `public int Health { get; private set; }` I like usage of property instead of readonly field 
because this will allow straighforward expansion of the code, i.e. adding events on health change, calculating health
using modifiers etc.
2. At the same time `health` is not enough to have full representation of player's health. You need at least one
additional variable `maxHealth` to not overflow during healing process and sufficiently display health bar.
3. `SetHealth` function does exactly what would custom property setter would.
4. `SetHealth` allows to set any health value to a player while in code it is used do deal damage. Better
approach would be to use `public int DealDamage(int value)` function where:
  - `value` is amount of damage we are trying to deal
  - return value represents how much damage were actually dealt which is useful in mechanics like shield,
  or damage reduction

# Example 1_2

1. In the previous example player's damage and health were hardcoded, which is unusable for game designers,
who wants to iterate quickly without recompiling game every change. Here the problem is solved by using
serialization - great improvement!
2. `SetHealth` was changed to `Hit`, also nice improvement.
3. For some reason `Settings` class include player damage, but doesn't include starting health. I would either
remove `Settings` class completely or added `startHealth` variable there.
4. As it is a console program we could benefit from `args` in `Main` function by passing there path to player config
file and settings file. If parameters were passed (or one of them) that path going to be preferrable instead of
hardcoded one.
5. Instead of creating player by simply parsing player config file I would live settings file only and pass
settings object to players constructor to initialize values through it `public Player(Settings settings)`

# Example 2_1

1. Move `OnPlayerHealthChanged` function to `class StatusTextView: TextView` which will include
`OnValueChanged(int oldValue, int newValue)` function. It will behave similar to `OnPlayerHealthChanged`
with a few tweaks:
  - `newHealth - oldHealth < -10` is reversed and not as easy to understand as `oldHealth - newHealth > 10`
  - instead of hardcoded color will be using gradient (which will require `maxValue` pass)
  - some minor thing: `Text = $"{newValue}/{oldValue}"` instead of just setting current health
2. `healthView.Text = player.Health.ToString();` should be moved to the class above in such way:
``` cs
class StatusTextView: TextView
{
    public Player Target
    {
        set(Player value)
        {
            target = value;
            OnValueChanged(target.Health, target.Health);
        }
    }
}
```

# Example 2_2

1. Custom accessor solves issue I described in previous example but in the other way. Quite a clever one, although
I would not expect a callback to be triggered as soon as I subscribed to it.
2. This solution introduced an issue of not being able to pass parameters, which results in creating `int? previousHealth` 
variable. Solution that I suggested above does not have this problem.
3. If for some reason there is no way to pass old health and new health through event we could use 0 as initial value
for `int previousHealth` (not nullable). `player.Health - previousHealth` will never be < -10 as `player.Health` is always
above or equal 0.

# Example 3_1

1. There is a list of walking points but nothing about tracking current point that player should walk to. So at least
`walkPointIndex` required here.
2. For some reason as soon as player reaches enemy, `currentEnemy` becomes `null`. Seems like a bug, because after
reaching enemy player should start attacking it or try to make peace. So it will definitely need `currentEnemy` set.
3. So instead of checking `if (currentEnemy != null)` it is better to check `if (isMoving)`.
4. `isMoving = activeWalkPath != null && activeWalkPath.Count > 0`.
5. Updating path to enemy every update is very dangerous to performance as it might be very complicated. So instead
we need to check if enemy moved with some threshold and only after that call `UpdatePathToEnemy`.

# Example 3_2

1. Not going to lie, I completely missed path generation written inside `UpdatePathToEnemy` function. Introduction
of `TryBuildPathToCoord` is a nice refactoring.
2. Everything else stays the same as for example above.

# Example 4_1

1. Very nice example of creating cheat commands. I believe this is actually a code from one of your projects :)
2. I don't like implementation of interface `ICheatProvider` by `SomeManagerWithCheats` class. Instead you
can use nested class:
``` cs
class SomeManager
{
    #if ENABLE_CHEATS
    class Cheats: ICheatProvider
    {
       // passing manager here to get access to all private functions and variables
       public Cheats(SomeManager manager) { }
       // GetCheatActions()
    }
    #endif
}
```
3. `CheatManager` itself could be `MonoBehaviour`. You will still need some `MonoBehaviour` class to call `Setup`
function for `CheatManager`. `DontDestroyOnLoad` obviously. This will clean code a bit by removing:
  - `Setup` function
  - `_panel` variable
  - need for instantiation of whole cheat panel every time you need to open it
Additionally by using a separate scene for `CheatManager` object you can setup custom building pipeline to
completely exclude cheats from the game. In this scene you can also add some debug panels like performance dynamic
analytic, in-game console, etc.

# Example 4_2

1. Very simplified approach to cheats comparing to previous example.
2. Every cheat right now *has* to be `MonoBehaviour` which creates additional step during setup by going to the
scene and not forgetting to add component. Additionally amount of cheats could be tens or even hundreds per system.
This will not make Unity's life easy loading all components for one object even in Editor.
3. For prototyping and testing systems is good enough.
