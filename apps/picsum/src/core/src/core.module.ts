import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './material.module';

import { Components } from './components';
import { ServerConfig } from './config';

@NgModule({
  declarations: [
    ...Components
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    MaterialModule
  ],
  exports: [
    ...Components,
    MaterialModule
  ]
})
export class CoreModule {
  static forRoot(config: ServerConfig): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers: [
        { provide: ServerConfig, useValue: config }
      ]
    }
  }
}
