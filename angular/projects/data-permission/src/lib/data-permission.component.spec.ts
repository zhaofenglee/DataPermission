import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { DataPermissionComponent } from './components/data-permission.component';
import { DataPermissionService } from '@j-s.Abp/data-permission';
import { of } from 'rxjs';

describe('DataPermissionComponent', () => {
  let component: DataPermissionComponent;
  let fixture: ComponentFixture<DataPermissionComponent>;
  const mockDataPermissionService = jasmine.createSpyObj('DataPermissionService', {
    sample: of([]),
  });
  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [DataPermissionComponent],
      providers: [
        {
          provide: DataPermissionService,
          useValue: mockDataPermissionService,
        },
      ],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataPermissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
