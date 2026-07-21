import { Routes } from '@angular/router';
import { MainLayout } from './core/layout/main-layout/main-layout';

export const routes: Routes = [
  {
    path: '',
    component: MainLayout,
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import(
            './features/dashboard/pages/dashboard-page/dashboard-page'
          ).then(component => component.DashboardPage)
      },
      {
        path: 'workspaces',
        loadComponent: () =>
          import(
            './features/workspaces/pages/workspace-list-page/workspace-list-page'
          ).then(component => component.WorkspaceListPage)
      },
      {
        path: 'boards',
        loadComponent: () =>
          import(
            './features/boards/pages/board-list-page/board-list-page'
          ).then(component => component.BoardListPage)
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'dashboard'
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];
