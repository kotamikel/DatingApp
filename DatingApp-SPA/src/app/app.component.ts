import { Component } from '@angular/core';

// Root Component being bootstraped by our app.module component
// Typescript + Decorators = Give angular features to JS 
@Component({
  // Config properties

  // Use selectors on other component templates to show children
  selector: 'app-root',
  // View of component
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'MIKELS APP';
}
