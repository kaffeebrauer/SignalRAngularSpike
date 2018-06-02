import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import './styles.scss';

if (environment.production) {
  enableProdMode();
}

declare var module: any;

if (module.hot) {
  module.hot.accept();
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.log(err));
