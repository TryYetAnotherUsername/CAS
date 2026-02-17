These classes here are super, super confusing.
Think of it like this:

| Class | Usage | Instance fields |
|-|-|-|
| `CatalogConfig` |  | • A dict consisting of: <br> Key- `string Name` <br> Value- `CatalogEntity [Object]`
| `CatalogEntity` |  | • `string Name` <br> • `string DispName` <br> • `PackedScene Scene` <br> • `Texture2D Thumbnail` <br> • `ECat Cat` <br> • `EPlacementMode PlacementMode` |
| `Prop` <br> *Inherits `Node3D`*|  | • `CatalogEntity Identity`|

And here is an attempt to understand this mess in human terms...

| Class | Usage | Instance fields |
|-|-|-|
| `CatalogConfig` <br> (indeed.com) | A list of all jobs. You can be a *Fireman*, a *Teacher*, etc.| • A dict consisting of: <br> Key- `string Name` <br> Value- `CatalogEntity [Object]` |
| `CatalogEntity` <br> (a job) | A job role — defines what a *Fireman* and their uniform is. | • `string Name` <br> • `string DispName` <br> • `PackedScene Scene` <br> • `Texture2D Thumbnail` <br> • `ECat Cat` <br> • `EPlacementMode PlacementMode` |
| `Prop` <br> (a person) | An person- John. He knows his `Identity`, a *Fireman*, and exists at a specific location | • `CatalogEntity Identity` |
| `Wall : Prop` <br> (a fireman) | A *Fireman* is a person therefore it inherits a person (`Prop`) | |

*Note: That last line is very forced, ohh well...*
Yea good luck with this timmy.