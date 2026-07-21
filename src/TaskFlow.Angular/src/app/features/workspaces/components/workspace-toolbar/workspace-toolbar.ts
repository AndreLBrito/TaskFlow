import { Component, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-workspace-toolbar',
  imports: [MatButtonModule, MatIconModule],
  templateUrl: './workspace-toolbar.html',
  styleUrl: './workspace-toolbar.scss',
})
export class WorkspaceToolbar {
  readonly createWorkspace = output<void>();
}
