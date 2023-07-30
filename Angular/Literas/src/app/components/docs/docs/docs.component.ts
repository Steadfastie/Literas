import {Component, OnDestroy, OnInit} from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'doc',
  templateUrl: './docs.component.html',
  styleUrls: ['./docs.component.sass']
})
export class DocsComponent implements OnInit, OnDestroy{

  constructor(private router: Router){
/*    this.router.navigate(['./create'])*/
  }
  ngOnInit(): void {
  }
  ngOnDestroy(): void {
  }
}
