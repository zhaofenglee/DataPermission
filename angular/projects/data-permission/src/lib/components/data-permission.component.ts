import { Component, OnInit } from '@angular/core';
import { DataPermissionService } from '../services/data-permission.service';

@Component({
  selector: 'lib-data-permission',
  template: ` <p>data-permission works!</p> `,
  styles: [],
})
export class DataPermissionComponent implements OnInit {
  constructor(private service: DataPermissionService) {}

  ngOnInit(): void {
    this.service.sample().subscribe(console.log);
  }
}
