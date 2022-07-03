# Advanced Tools - Tessellation

![Alt text](https://i.ibb.co/58V8ZFp/Water-Shader.png)

## Introduction

By definition Tessellation is tiling that uses shapes to cover a surface with no gaps or overlaps.  
When using tessellation in a shader, it covers the basics of that statement but it really is a way to subdivide meshes.
It allows a triangle to be divided into smaller triangles, and those new triangles can introduce a new level of detail
that wouldn't otherwise be possible with the undivided triangle.

Of course, you could also just use a higher triangle mesh. It would save the shader some time as the level of detail
would already be at the aimed level. But is this actually better?

**Evaluation Proposal:** Compare Tessellation and actual pre-made high triangle meshes in their performance in water-like shaders using Vertex Offset and see which one yields better FPS results in both a VR and a normal environment.

## Tessellation

// Tessellation Description


## Test Setup

All the tests were done inside the Unity Editor. The testing was automated with set start & stop conditions to allow for a simple workflow. Each test generated a log file, and added a small entry to (in the project referred to as) the Multilog which gives a simple overview of all tests run.

All tests were done using the same amount of meshes and the same amount of triangles. For the pre-triangulated meshes (without Tessellation), a custom mesh is created which will have the same amount of triangles as the tessellated mesh. All meshes are one sided planes with a simple vertex offset (sin wave) & color differences to simulate simple waves.

Both the VR and the normal tests were done on the exact same test setup, although a custom camera (XR-Origin from OpenXR) was used for the VR setup. When running VR, two cameras render each image, meaning each triangle gets rendered twice. This takes a heavy hit on performance. All tests were done with the game resolution set to 4K in the editor. 

This setup was created mainly using this [Unity Example](https://docs.unity3d.com/Manual/SL-SurfaceShaderTessellation.html), and was expanded upon using parts from this [Tutorial](https://youtu.be/63ufydgBcIk) and this [Tutorial Series](https://www.youtube.com/watch?v=Sr2KoaKN3mU). The Unity Documentations were also often consulted for other aspects such as the creation of log files. 

### PC Specifications
- OS: [Windows 10 Home](https://www.microsoft.com/en-US/d/windows-10-home/d76qx4bznwk4)
- CPU: [Intel i7-8086K](https://www.intel.com/content/www/us/en/products/sku/148263/intel-core-i78086k-processor-12m-cache-up-to-5-00-ghz/specifications.html)
- GPU: [NVIDIA GeForce GTX 1080 Ti](https://www.nvidia.com/en-us/geforce/products/10series/ultimate-4k/)
- RAM: [32GB (2x16) 2400MHz](https://www.alternate.nl/Corsair/32-GB-DDR4-2400-Kit-werkgeheugen/html/product/1262872)
- SSD: [Samsung SSD 970 Pro 1TB](https://www.amazon.com/Samsung-970-PRO-Internal-MZ-V7P1T0BW/dp/B07BYHGNB5)
- MOBO: [ASUS PRIME Z370-p](https://www.asus.com/us/motherboards-components/motherboards/prime/prime-z390-p/)
- VR Headset: [Valve Index](https://www.valvesoftware.com/en/index)

### Project Specifications
- Unity Hub Version: 3.2.0
- Unity Editor Version: 2021.3.1f1
- Unity License: Professional

## The Tests

**All tests were done using the following setup:**

![Alt image](https://i.ibb.co/HDSzgHx/Unity-Setup.png)

There are three different steps to setting up a test:
- First a choice must be made of which meshes to run. Within the hirarchy there are different options for different amounts of triangulated(with the name Triangle) or Tessellated (with the name Plane) meshes. To test a setup, one of them must be enabled.
- Next in the Tessellation material a starting amount must be selected to start tessellating from.
- Finally, in the Log GameObject all settings for the run can be tweaked. The amount of runs can be selected, how long it will take per run, which mesh type to log for (triangular or tessellation) & whether it should increase in tessellation after all runs are over, which allows it to automatically run through all different amounts. Once all settings are tweaked, the Start Test button can be pressed which will start the tests with the settings provided.

After each test, a log file is generated within the Logs folder, and an entry to the Multilog will give a more brief information regarding the test.

**Example Log File:**

![Log](https://i.ibb.co/qmVLYXT/Log.png)

**MultiLog:**

![MultiLog](https://i.ibb.co/cDwpk1H/Multilog.png)

### Test 1: Normal

The normal test setup uses 20.000 triangles to start out with, to bring down start FPS to about 110-120 FPS. From there for each triangle amount 10 tests were done each consisting of 10 seconds. The tessellation amount was increased by 2 each time (As Unity uses an Uneven paritioning mode all values are Uneven). After running the first tests I realized that I could start incrementing by 4 as 2 was too little of a difference, and at the end values were incremented by 8. 

In total 25 different Tessellation amounts have been tested for both the Tessellated Meshes & The Triangulated Meshes. This means a total of 50 test runs have been completed for this test, each consisting of 10 tests to get a reliable average. In total 500 runs have been done and documented. 

In the following graph the averages of each run can be seen for both the Tessellated Meshes (orange) and Triangulated Meshes (gray).

#### Test 1 Graph
![GraphTest1](https://i.ibb.co/0Bgzj3n/Graph1.png)

From this graph it shows clearly that triangulated & tessellated meshes perform very similarly until a certain point, from where Tessellation becomes a lot more efficient. Around the 8.660.000 triangles the triangulated meshes start declining in performance, which keeps decreasing when more triangles are added. The tessellated meshes however have a massive dip around 60.740.000 triangles, and stabalize around 216.740.000 triangles. The FPS for the Tessellated meshes stays very managable, even under extreme conditions, whereas the Triangulated Meshes very soon starts dipping below acceptable FPS. Of course, this very much depends on the test setup, but here the tessellation managed to keep performance for a lot longer.


The following tables show all the data gathered:

#### Main Data - Test 1

![MainTable](https://i.ibb.co/B3jbLrB/Main-Table.png)

##### Tessellation Data - Test 1

![TessTable](https://i.ibb.co/1dyBngK/Tessellation-Table.png)

##### Triangle Data - Test 1

![TriTable](https://i.ibb.co/tJJTgHv/Triangle-Table.png)

### Test 2: VR

The tests for VR were very similarly set up to the first tests. An XR-Origin (OpenXR) was introduced & the project was converted into a XR project to allow for the usage of a VR headset. The same 20.000 start-amount was used. For each test however, wearing the headset was required. This meant that for each test a person had to wear the headset while the tests ran, and had to look forward. Due to this reason a lot less tests were run, as even starting the scene would take an additional ~20-30 seconds per run. For the VR tests 7 different values were tested for both Tessellation & Triangle, each consiting of 3 runs of 10 seconds. This meant that a total of 42 tests have been done.

The following graph shows the results for the second test in VR:

### Test 2 Graph

![GraphTest2](https://i.ibb.co/QCNdK3v/Graph2.png)

This graph yields very similar but also very different results from the first test. Although there are the same amount of triangles, each triangle is rendered twice. This corresponds to roughly half the FPS and the drop in FPS being around half of what happens in a normal environment.

The real drop for the triangulated meshes happens around 18.740.000 triangles. It steeply declines and sticks to declining afterwards. The tessellated meshes, similarly to the first test, stay performant for much longer, and only start seeing a real drop after 41.060.000 triangles. It keeps a higher average FPS overall, and although FPS is for VR not acceptable at any moment, Tessellation seems to still be a lot more effective at rendering in higher subdivisions.

The following tables show all the data gathered:

#### Main Data - Test 2

![MainTable](https://i.ibb.co/WyHSNLG/Main-Table2.png)

##### Tessellation Data - Test 2

![TessTable](https://i.ibb.co/4gwYVWC/Tess-Table2.png)

##### Triangle Data - Test 2

![TriTable](https://i.ibb.co/r614Y6c/Tri-Table2.png)

## Conclusion

// Conclusion

## Retrospective

// Retrospective

The lower amount of runs means that it is less reliable. In a seperate future test I would want to see if just rendering two cameras would yield the same amount of FPS as running the game in VR, meaning you could use 