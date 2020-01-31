

export class PersonBE {

    constructor(personId?: string, name?: string) {

        this.name = name;
        this.personId = personId;
    }
    

    public personId: string;
    public userId: string;
    public identityCardNumberType: number;
    public identityCardNumber: string;
    public lastName: string;
    public name: string;
    public sex: number;
    //IdEstadocivil: number;
    public birthDate?: Date;

    public entryDate: Date;
    public photo: ArrayBuffer;
    public email: string;
    public phone1: string;
    public phone2: string;




    public FullName(): string {
        return PersonBE.getFullName(this.lastName, this.name);
    }
    public static getFullName(firstName: string, lastName: string): string {
        if ((lastName || lastName != '') && (firstName || firstName != ''))
            return lastName.trim(), ", ", firstName.trim();

        if ((!lastName || lastName == '') && (firstName || firstName != ''))
            return firstName.trim();

        if ((lastName || lastName == '') && (!firstName || firstName == ''))
            return lastName.trim();

        return '';
    }

    public places: PlaceBE[];
    public addressBEList: AddressBE[];

    public lastAccessTime: Date;
    public lastAccessUserId: string;
    public lastUserNameUpdate: string;
    public status?: number;

    public phone1_label: string;
    public phone2_label: string;
    public mail_label: string;


}

export class PlaceBE {
    public adr_address: string;
    public formatted_address: string;
    public id: string;
    public place_id: string;
    public Street: string;
    public StreetNumber: number;
    public mail: string;
    public Telefono1: string;
    public Telefono2: string;
    public Floor: string;
}

export class AddressBE {

    public addressId :number;
    public personId: string;
    public street: string;
    public streetNumber: number;
    public countryId: number;
    public provinceId: number;
    public cityId: number;
    public neighborhood: string;
    public province: string;
    public city: string;
    public zipCode: string;
    public floor: string;

    public state:number;
}