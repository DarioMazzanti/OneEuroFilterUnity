# One Euro Filter utility for Unity (beta)

*OneEuroFilter for Unity* is a simple utility for filtering noisy signals, built upon the [OneEuroFilter] by [Géry Casiez], [Nicolas Roussel] and [Daniel Vogel]. It is rewritten as a C# class for [Unity], starting from the [C++ implementation] by [Nicolas Roussel]. 

*This utility was written to allow the easy filtering of some Unity data types, such as Vector2, Vector3, Vector3 and Quaternion...and of course float. It is not particularly polished: suggestions are welcome! :)*

## About
The 1€ filter is described in the [CHI 2012 paper] by [Géry Casiez], [Nicolas Roussel] and [Daniel Vogel]: a precise and responsive algorithm for filtering noisy signals, particularly suited for interactive systems. A really nice [online version] by [Jonathan Aceituno] is available to try.

## Acknowledgements
Thanks to [marcbal] for the contribution to Quaternion filtering for non-continuous input Quaternions!

## Content and Installation
A test project named *OneEuroFilterUnity* is available. The project contains two scenes:

- **TestScene0**: random noise is applied to the position (Vector3 type) of a cube floating along its vertical axis.
- **TestScene1**: random noise is applied to the rotation (Quaternion type) of a rotating cube.

The *OneEuroFilteringTest* object existing in both scenes allows you to turn the 1€ filter on/off, to change its parameters and to set the amount of random noise applied to the cube on the left of the scene. You can see the result of the filtering applied to the cube on the right. The project was developed using Unity 5.3.4f1, but the utility itself is just a .cs file with no external dependencies, so it should work for other Unity versions as well (previous and next, I guess).  
> If you want to use the filter in your own projects (that's the idea, right?!) just copy the [OneEuroFilter.cs] file to your own Assets folder. 

## Usage
In the following example a noisy float and Vector3 are being filtered. The filtered Vector3 is used as the new position for a Transform:
```cs
using UnityEngine;
using System.Collections;

public class FilteredObject : MonoBehaviour
{
    // noisy inputs coming from somewhere
    public float noisyFloat;
    public Vector3 noisyVector3;

    // the filters
    OneEuroFilter floatFilter;
    OneEuroFilter<Vector3> vector3Filter;
    
    // filter(s) frequency
    public float filterFrequency = 120.0f;
    
    // INITIALIZATION
    void Start()
    {
        // class constructor initializes the filter(s) with the provided frequency (and standard parameters)
        floatFilter = new OneEuroFilter(filterFrequency);
        vector3Filter = new OneEuroFilter<Vector3>(filterFrequency);
    }
    
    // let's pretend noisyFloat and noisyVector3 have been updated somewhere else...
    void Update()
    {
        //-------------------------------------------------------------
        // FILTERING OUR FLOAT SIGNAL (NOT USING IT)
            float filteredFloat = floatFilter.Filter(noisyFloat);
        //-------------------------------------------------------------
        
        //-------------------------------------------------------------
        // FILTERING THE VECTOR3 CONTAINING OUR SIGNAL (AND USING IT)
            // filter the signal
            Vector3 filteredInput = positionFilter.Filter(noisyVector3);
            
            // apply the signal to this objects transform position
            transform.position = filteredInput;
        //-------------------------------------------------------------
    }
}
```
Please note that in this example the only parameter set by the constructors is the filter frequency. This OneEuroFilter C# implementation is based on the [C++ implementation] by [Nicolas Roussel], so it is possible to specify the minimum cutoff frequency, the cutoff slope (beta) and the derivate cutoff frequency (see [OneEuroFilter] documentation for more information). It is also possible to provide a timestamp when filtering data, thus updating the frequency of the filter based on that.

### Available Functions
They are not many, and they are used for instantiating the filters, updating their parameters and filtering the data.

- **Class constructor**  
    It's the same for all the available data types (float, Vector2, Vector3, Vector4 and Quaternion). Frequency argument is mandatory.
    ```cs
    public OneEuroFilter(float _freq, float _mincutoff=1.0f, float _beta=0.0f, float _dcutoff=1.0f) {...}
    
    // e.g. for float and Vector3
    OneEuroFilter floatFilter = new OneEuroFilter(100.0f, 1.0f, 0.001f, 1.0f);
    OneEuroFilter<Vector3> vec3Filter = new OneEuroFilter<Vector3>(100.0f, 1.0f, 0.001f, 1.0f);
    ```

- **Updating Parameters**  
    It is - very much like the constructor - the same for all data types. It allows to dynamically update the filter parameters.
    ```cs
    public void UpdateParams(float _freq, float _mincutoff = 1.0f, float _beta = 0.0f, float _dcutoff = 1.0f) {...}
    
    // e.g.
    myFilter.UpdateParams(70.0f, 1.0f, 0.0f, 1.0f);
    ```

- **Actual Filtering**  
    This is what we're interested in! These functions filter the value received as argument, and return the result. Two definitions are needed in order for the utility to work with multiple datatypes, but their usage is the same: ourFilter.Filter(noisySignal).
    ```cs
    // float filtering
    public float Filter(float value, float timestamp = -1.0f) {...}
    // all other available data types filtering
    public T Filter<T>(T _value, float timestamp = -1.0f) {...}
    
    // e.g for float and Vector3
    float filteredFloat = myFilter.Filter(noisyFloat);
    Vector3 filteredeVec3 = myVec3Filter.Filter(noisyVec3);
    ```

## Tested
Windows 10 + Unity 5.3.4f1

## Dependencies
- Unity
- Some noisy signals to filter :)

## Development
The OneEuroFilter is available in many programming languages and within a number of coding environments. Nonetheless, I wasn't able to find a Unity version, so I made a rather simple one which suites my projects needs. Some bugs may definitely be there and it could lack some features which can be essential for someone else. It is fastly written, so the code may not be state of the art: comments and suggestions are welcome!

## Todos
 - Further Testing
 - Testing on Mac and other Windows versions
 - Code Revision
 - Revise/add Code Comments
 - js version (only if needed)
  
___
This utility is developed and maintained by [Dario Mazzanti](https://www.iit.it/people/dario-mazzanti).  
*This README file was last updated on 2016-05-24 by Dario Mazzanti.*





[OneEuroFilter]: <http://www.lifl.fr/~casiez/1euro/>
[Géry Casiez]: <http://cristal.univ-lille.fr/~casiez/>
[Daniel Vogel]: <http://www.nonsequitoria.com/>
[Unity]: <https://unity3d.com/>
[C++ implementation]: <http://www.lifl.fr/~casiez/1euro/OneEuroFilter.cc>
[Nicolas Roussel]: <http://interaction.lille.inria.fr/~roussel/>
[CHI 2012 paper]: <http://www.lifl.fr/~casiez/publications/CHI2012-casiez.pdf>
[online version]: <http://www.lifl.fr/~casiez/1euro/InteractiveDemo/>
[Jonathan Aceituno]: <http://p.oin.name/>
[OneEuroFilter.cs]: <https://github.com/DarioMazzanti/OneEuroFilterUnity/blob/master/Assets/Scripts/OneEuroFilter.cs>
[marcbal]: <https://github.com/marcbal>
