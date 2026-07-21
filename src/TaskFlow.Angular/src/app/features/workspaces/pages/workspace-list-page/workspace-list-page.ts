import { AsyncPipe, JsonPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { WorkspaceService } from '../../services/workspace.service';

@Component({
  selector: 'app-workspace-list-page',
  imports: [
    AsyncPipe,
    JsonPipe
  ],
  templateUrl: './workspace-list-page.html',
  styleUrl: './workspace-list-page.scss'
})
export class WorkspaceListPage {
  private readonly workspaceService = inject(WorkspaceService);

  readonly workspaces = this.workspaceService.getAll();
}
