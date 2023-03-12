import { Component } from '@angular/core';
import Quill from "quill";

let Inline = Quill.import('blots/inline');
class SubHeaderBlot extends Inline {
  static create() {
    let node = super.create();
    node.setAttribute('class', 'subheader');
    node.add("");
    return node;
  }
}
SubHeaderBlot["blotName"] = 'subHeader';
SubHeaderBlot["tagName"] = 'div';
Quill.register(SubHeaderBlot);

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent {
  title = 'Literas';
}
