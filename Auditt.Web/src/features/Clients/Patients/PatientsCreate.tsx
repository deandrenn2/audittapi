import { useRef } from "react";
import { usePatients } from "./UsePatients";
import { PatientsModel } from "./PantientsModel";

type formData = {
    
    firstName: string;
    lastName: string;
    documentNumber: string;
    birthDate: string;
    eps: string;
};

export const PatientsCreate = () => {
    const { createPatients } = usePatients();
    const refForm = useRef<HTMLFormElement>(null);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const form = e.target as HTMLFormElement;
        const formData = new FormData(form);
        const patient = Object.fromEntries(formData.entries()) as formData;
        const newPatient: PatientsModel = {
            id: 0,
            firstName: patient.firstName ?? "",
            lastName: patient.lastName ?? "",
            documentNumber: patient.documentNumber ?? "",
            birthDate: patient.birthDate ?? "",
            eps: patient.eps ?? "",
        };

        const response = await createPatients.mutateAsync(newPatient);

        if (response.isSuccess) {
            refForm.current?.reset();
        }
    };

    return (
        <div>
            <form ref={refForm} className="space-y-4" onSubmit={handleSubmit}>
                <div>
                    <label className="block text-sm font-medium mb-1">Nombre</label>
                    <input
                        type="text"
                        name="firstName"
                        required
                        className="w-full border border-gray-300 rounded px-2 py-2"
                    />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Apellido</label>
                    <input
                        type="text"
                        name="lastName"
                        required
                        className="w-full border border-gray-300 rounded px-3 py-2"
                    />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Número de Documento</label>
                    <input
                        type="text"
                        name="documentNumber"
                        required
                        className="w-full border border-gray-300 rounded px-3 py-2"
                    />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Fecha de Nacimiento</label>
                    <input
                        type="date"
                        name="birthDate"
                        required
                        className="w-full border border-gray-300 rounded px-3 py-2"
                    />
                </div>
                <div>
                    <label className="block text-sm font-medium mb-1">Eps</label>
                    <input
                        type="text"
                        name="eps"
                        required
                        className="w-full border border-gray-300 rounded px-3 py-2"
                    />
                </div>
                <div>
                    <button
                        type="submit"
                        className="bg-indigo-500 hover:bg-indigo-900 text-white px-8 py-2 rounded-lg font-semibold"
                    >
                        {createPatients.isPending ? "Creando..." : "Crear"}
                    </button>
                </div>
            </form>
        </div>
    );
};
