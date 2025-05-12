export interface AssessmentCreateModel {
	idInstitution: number;
	idDataCut: number;
	idFunctionary: number;
	idPatient: number;
	idGuide: number;
	yearOld?: string;
	date: Date;
	eps: string;
	idUser: number;
	id?: number;
}

export interface AssessmentListModel {
	id: number;
	identificationPatient: string;
	functionaryName: string;
	date: Date;
}

export interface AssessmentModel {
	idDataCut: number;
	idFunctionary: number;
	idPatient: number;
	idInstitution: number;
	identity: string;
	idGuide: number;
}

export interface AssessmentDetailModel {
	id: number;
	idDataCut: number;
	idFunctionary: number;
	idPatient: number;
	date: string;
	eps: string;
	yearOld: string;
	idUserCreated: number;
	idUserUpdate: number;
	updateDate: string;
	createDate: string;
	valuations: ValuationModel[];
	idScale: number;
}

export interface ValuationModel {
	id: number;
	order: number;
	text: string;
	idAssessment: number;
	idEquivalence: number;
	idQuestion: number;
}

export interface AssessmentValuationsModel {
	id: number;
	idPatient: number;
	yearOld: string;
	date: string; // Fecha ISO, ej. "2025-05-09T14:15:22Z"
	eps: string;
	valuations: ValuationModel[];
	idUser: number;
}
