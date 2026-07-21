import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiService } from '../../../core/api/api.service';
import { Workspace } from '../../../core/models/workspace';

@Injectable({
    providedIn: 'root'
})
export class WorkspaceService extends ApiService {

    getAll(): Observable<Workspace[]> {

        return this.http.get<Workspace[]>(
            `${this.apiUrl}/workspaces`);
    }

}
