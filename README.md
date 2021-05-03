# 3D-Mesh-Visualizer

Any Unity 2020.3 LTS
Scene: Scenes > MeshVisualizer
Plateform: iOS / Android

## Scene Overview

#### Canvas-MeshVisualizer

UI for the Mesh Visualizer. The component "CanvasSafeArea" is used to place the UI inside the safe area of a phone, avoiding notches and rounded corners.
The SideMenuController handles the logic of the side menu and dynamic laoding of models UI.

#### Input Controller

Input logic to manipulate a mesh using touch input. 
The pivot gameObject is a child of this gameObject.

#### Model Controller

Load And display models by adding ModelData to the Model List.

#### Background Controller

Change the skybox color using Background data scriptable object.

#### Lighting Controller

Change the directionnal light to day or night along with the ambient color and another light mode.

## Adding a new model to the visualizer

1. Create an Empty GameObect
2. Add the model as child of this empty
3. It's recommended that the model fits in a 1x1 unity box, so it's similar to the other models
4. Add a boxCollider component to the empty and set the bounds to fit the mesh. You can also add some padding to the bounds
5. Create a prefab with this empty
6. Create a ModelData scriptableObject in Data > ModelData by right clicking and selecting Create > Model Data
7. Fill in the name and select the created prefab. You can also force a Post processing profile when the model is selected and automatically change the background color.
8. Select you ModelData and add it to Model List in the ModelController script.


## Plugin and Packages

- Device Simulator 2.2.4-preview
- DOTween for quick animation - As the artistic skills doesn't count I added DOTween for a bit of polish during some animations.
