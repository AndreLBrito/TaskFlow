import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { finalize } from 'rxjs';

import { Workspace } from '../../../../core/models/workspace';
import { WorkspaceTable } from '../../components/workspace-table/workspace-table';
import { WorkspaceToolbar } from '../../components/workspace-toolbar/workspace-toolbar';
import { WorkspaceService } from '../../services/workspace.service';

@Component({
  selector: 'app-workspace-list-page',
  imports: [
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    WorkspaceTable,
    WorkspaceToolbar,
  ],
  templateUrl: './workspace-list-page.html',
  styleUrl: './workspace-list-page.scss'
})
export class WorkspaceListPage {
  private readonly workspaceService = inject(WorkspaceService);
  private readonly destroyRef = inject(DestroyRef);

  readonly workspaces = signal<Workspace[]>([]);
  readonly loading = signal(true);
  readonly error = signal(false);

  constructor() {
    this.loadWorkspaces();
  }

  loadWorkspaces(): void {
    this.loading.set(true);
    this.error.set(false);

    this.workspaceService
      .getAll()
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => this.loading.set(false)),
      )
      .subscribe({
        next: workspaces => this.workspaces.set(workspaces),
        error: () => {
          this.workspaces.set([]);
          this.error.set(true);
        },
      });
  }

  createWorkspace(): void {
    console.log('Create workspace requested.');
  }

  editWorkspace(workspace: Workspace): void {
    console.log('Edit workspace requested:', workspace);
  }

  deleteWorkspace(workspace: Workspace): void {
    console.log('Delete workspace requested:', workspace);
  }
}
